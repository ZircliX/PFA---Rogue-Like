using System.Collections.Generic;
using DeadLink.PowerUpSystem;
using DeadLink.Save.Settings;
using SaveSystem.Core;
using UnityEngine;
using ZLinq;

namespace RogueLike.Save
{
    public class SaveController : ISaveController<GameProgression> , ISaveController<SettingsSave>
    {
        private const string SAMPLE_SAVE_FILE = "SaveFile";
        private const string SETTINGS_SAVE_FILE = "SettingsSaveFile";

        public GameProgression GetDefaultSaveFile() => new GameProgression()
        {
            DifficultyData = GameMetrics.Global.NormalDiffuculty,
            
            HealthPoints = 100,
            HealthBarCount = 3,
            
            LastCheckPoint = null,
            EnemyPositions = new List<Transform>(),
            
            RemainingPowerUps = GameMetrics.Global.PowerUps.AsValueEnumerable().ToList(),
            PlayerPowerUps = new List<PowerUp>(),
        };

        public bool Push(in GameProgression saveFile)
        {
            string json = JsonUtility.ToJson(saveFile);
            PlayerPrefs.SetString(SAMPLE_SAVE_FILE, json);
            return true;
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

        public bool Pull(out GameProgression saveFile)
        {
            saveFile = default;
            if (!PlayerPrefs.HasKey(SAMPLE_SAVE_FILE))
                return false;

            string json = PlayerPrefs.GetString(SAMPLE_SAVE_FILE);
            saveFile = JsonUtility.FromJson<GameProgression>(json);
            return true;
        }

        SettingsSave ISaveController<SettingsSave>.GetDefaultSaveFile()
        {
            return new SettingsSave()
            {
                SfxVolume = 1f,
                MusicVolume = 1f,
                VoiceVolume = 1f
            };
        }
    }
}