using System.Collections;
using FMODUnity;
using LTX.Singletons;
using TMPro;
using UnityEngine;

namespace DeadLink.VoiceLines
{
    public class VoiceLinesManager : MonoSingleton<VoiceLinesManager>
    {
        [field: SerializeField] public EventReference PlayerHitEvent { get; private set; }
        [field: SerializeField] public VoiceLineData PlayerHitData { get; private set; }
        [field: SerializeField] public EventReference[] AudioEvents { get; private set; }
        [field: SerializeField] public VoiceLineData[] VoiceLineData { get; private set; }

        [SerializeField] private Transform player;
        [SerializeField] private TMP_Text dialogueText;

        private bool playerHit;
        private Coroutine dialogueCoroutine;
        private int dialogueIndex = -1;
        
        public void PlayVoiceLine(int voiceLinesIndex)
        {
            if (voiceLinesIndex <= dialogueIndex) return;
            dialogueIndex = voiceLinesIndex;
            
            AudioManager.Global.StopVoices();
            if (dialogueCoroutine != null)
                StopCoroutine(dialogueCoroutine);
            
            AudioManager.Global.PlayOneShot(AudioEvents[voiceLinesIndex], player.position);
            dialogueCoroutine = StartCoroutine(DialogueDuration(VoiceLineData[voiceLinesIndex].DialogueDuration));
        }

        public void PlayVoiceLineWithDialogue(int voiceLinesIndex)
        {
            if (voiceLinesIndex <= dialogueIndex) return;
            dialogueText.text = VoiceLineData[voiceLinesIndex].Dialogue;
            PlayVoiceLine(voiceLinesIndex);
        }

        public void PlayerHit()
        {
            if (playerHit || PlayerHitEvent.IsNull) return;
            playerHit = true;
            
            AudioManager.Global.StopVoices();
            if (dialogueCoroutine != null)
                StopCoroutine(dialogueCoroutine);
            
            AudioManager.Global.PlayOneShot(PlayerHitEvent, player.position);
            dialogueCoroutine = StartCoroutine(DialogueDuration(PlayerHitData.DialogueDuration));
            dialogueText.text = PlayerHitData.Dialogue;
        }

        private IEnumerator DialogueDuration(float duration)
        {
            yield return new WaitForSeconds(duration);
            dialogueText.text = string.Empty;
            //playerHit = false;
        }
    }
}