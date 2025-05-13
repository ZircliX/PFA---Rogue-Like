using SaveSystem.Core;

namespace DeadLink.Save.Settings
{
    public struct SettingsSave : ISaveFile
    {
        public int Version { get; }
        public float SfxVolume { get; set; }
        public float MusicVolume { get; set; }
        public float VoiceVolume { get; set; }
    }
}