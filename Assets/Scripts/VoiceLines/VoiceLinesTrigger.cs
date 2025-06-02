using UnityEngine;

namespace DeadLink.VoiceLines
{
    public class VoiceLinesTrigger : MonoBehaviour
    {
        [SerializeField] private int voiceLinesIndex;
        
        private void OnTriggerEnter(Collider other)
        {
            VoiceLinesManager.Instance.PlayVoiceLineWithDialogue(voiceLinesIndex);
        }
    }
}
