using UnityEngine;

namespace DeadLink.VoiceLines
{
    [CreateAssetMenu(fileName = "VoiceLines", menuName = "Voice Lines")]
    public class VoiceLineData : ScriptableObject
    {
        [TextArea] public string Dialogue;
        public float DialogueDuration = 2f;
    }
}