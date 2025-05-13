using RogueLike.Controllers;
using SaveSystem.Core;

namespace DeadLink.Save.Settings
{
    public class SettingsListener : ISaveListener<SettingsSave>
    {
        public int Priority { get; }
        
        public float SfxVolume { get; private set; }
        public float MusicVolume { get; private set; }
        public float VoiceVolume { get; private set; }

        public void Write(ref SettingsSave saveFile)
        {
            saveFile.MusicVolume = MusicVolume;
            saveFile.SfxVolume = SfxVolume;
            saveFile.VoiceVolume = VoiceVolume;
        }

        public void Read(in SettingsSave saveFile)
        {
            MusicVolume = saveFile.MusicVolume;
            SfxVolume = saveFile.SfxVolume;
            VoiceVolume = saveFile.VoiceVolume;
        }
        
        public void SetSfxVolume(float volume)
        {
            SfxVolume = volume;
        }
        
        public void SetMusicVolume(float volume)
        {
            MusicVolume = volume;
        }
        
        public void SetVoiceVolume(float volume)
        {
            VoiceVolume = volume;
        }
    }
}