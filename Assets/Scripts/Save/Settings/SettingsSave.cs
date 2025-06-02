using SaveSystem.Core;
using UnityEngine;

namespace DeadLink.Save.Settings
{
    public struct SettingsSave : ISaveFile
    {
        public int Version { get; }
        
        [SerializeField] public float SfxVolume;
        [SerializeField] public float MusicVolume;
        [SerializeField] public float VoiceVolume;
    }
}