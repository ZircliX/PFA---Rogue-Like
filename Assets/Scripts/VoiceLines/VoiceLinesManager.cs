using System.Collections;
using FMODUnity;
using LTX.Singletons;
using RogueLike.Managers;
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

        private bool active = true;
        
        public void PlayVoiceLine(int voiceLinesIndex)
        {
            if (!active) return;
            if (LevelManager.Instance.PlayerController.PlayerEntity.PowerUps.Count > 0) return;
            if (voiceLinesIndex <= dialogueIndex) return;
            dialogueIndex = voiceLinesIndex;
            
            AudioManager.Global.StopVoices();
            AudioManager.Global.PlayOneShot(AudioEvents[voiceLinesIndex], player.position);
        }

        public void PlayVoiceLineWithDialogue(int voiceLinesIndex)
        {
            if (!active) return;
            if (LevelManager.Instance.PlayerController.PlayerEntity.PowerUps.Count > 0) return;
            if (voiceLinesIndex <= dialogueIndex) return;
            
            dialogueText.text = VoiceLineData[voiceLinesIndex].Dialogue;
            if (dialogueCoroutine != null)
                StopCoroutine(dialogueCoroutine);
            dialogueCoroutine = StartCoroutine(DialogueDuration(VoiceLineData[voiceLinesIndex].DialogueDuration));
            
            PlayVoiceLine(voiceLinesIndex);
        }

        public void PlayerHit()
        {
            if (!active) return;
            if (playerHit || PlayerHitEvent.IsNull) return;
            playerHit = true;
            
            AudioManager.Global.StopVoices();
            if (dialogueCoroutine != null)
                StopCoroutine(dialogueCoroutine);

            if (player == null) return;
            if (!PlayerHitEvent.IsNull)
            {
                AudioManager.Global.PlayOneShot(PlayerHitEvent, player.position);
                dialogueCoroutine = StartCoroutine(DialogueDuration(PlayerHitData.DialogueDuration));
                dialogueText.text = PlayerHitData.Dialogue;
            }
        }

        public void SelectPower()
        {
            if (!active) return;
            if (LevelManager.Instance.PlayerController.PlayerEntity.PowerUps.Count > 0) return;

            PlayVoiceLine(1);
        }
        
        public void SelectedPower()
        {
            if (!active) return;
            if (LevelManager.Instance.PlayerController.PlayerEntity.PowerUps.Count > 0) return;

            PlayVoiceLine(2);
        }

        private IEnumerator DialogueDuration(float duration)
        {
            yield return new WaitForSeconds(duration);
            dialogueText.text = string.Empty;
            //playerHit = false;
        }

        public void SetActiveState(bool enableVoicelines)
        {
            active = enableVoicelines;
        }
    }
}