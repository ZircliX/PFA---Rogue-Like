using SaveSystem.Core;
using UnityEngine;

namespace DeadLink.Save.Settings
{
    public class SettingsSaveController : ISaveController<SettingsSave>
    {
        private const string SETTINGS_SAVE_FILE = "SettingsSaveFile";
        
        public SettingsSave GetDefaultSaveFile()
        {
            return new SettingsSave()
            {
                SfxVolume = 5f,
                MusicVolume = 5f,
                VoiceVolume = 5f
            };
        }
        
        public bool Push(in SettingsSave saveFile)
        {
            string json = JsonUtility.ToJson(saveFile);
            PlayerPrefs.SetString(SETTINGS_SAVE_FILE, json);
            return true;
        }

        public bool Pull(out SettingsSave saveFile)
        {
            saveFile = default;
            if (!PlayerPrefs.HasKey(SETTINGS_SAVE_FILE))
                return false;

            string json = PlayerPrefs.GetString(SETTINGS_SAVE_FILE);
            saveFile = JsonUtility.FromJson<SettingsSave>(json);
            return true;
        }
    }
}