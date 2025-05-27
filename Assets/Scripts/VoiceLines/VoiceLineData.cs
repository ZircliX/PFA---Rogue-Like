using UnityEngine;

namespace DeadLink.VoiceLines
{
    [CreateAssetMenu(fileName = "VoiceLines", menuName = "Voice Lines")]
    public class VoiceLineData : ScriptableObject
    {
        public string Dialogue;
    }
}