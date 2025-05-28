using FMODUnity;
using LTX.Singletons;
using TMPro;
using UnityEngine;

namespace DeadLink.VoiceLines
{
    public class VoiceLinesManager : MonoSingleton<VoiceLinesManager>
    {
        [field: SerializeField] public EventReference[] AudioEvents { get; private set; }
        [field: SerializeField] public EventReference PlayerHitEvent { get; private set; }
        [field: SerializeField] public VoiceLineData[] VoiceLineData { get; private set; }

        [SerializeField] private Transform player;
        [SerializeField] private TMP_Text dialogueText;

        private bool playerHit;
        
        public void PlayVoiceLine(int voiceLinesIndex)
        {
            AudioManager.Global.PlayOneShot(AudioEvents[voiceLinesIndex], player.position);
        }

        public void PlayVoiceLineWithDialogue(int voiceLinesIndex)
        {
            dialogueText.text = VoiceLineData[voiceLinesIndex].Dialogue;
            PlayVoiceLine(voiceLinesIndex);
        }

        public void PlayerHit()
        {
            if (playerHit || PlayerHitEvent.IsNull) return;
            playerHit = true;
            AudioManager.Global.PlayOneShot(PlayerHitEvent, player.position);
        }
    }
}