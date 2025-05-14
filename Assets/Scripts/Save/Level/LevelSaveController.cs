using System.Collections.Generic;
using DeadLink.Extensions;
using SaveSystem.Core;
using UnityEngine;

namespace DeadLink.Save.Level
{
    public class LevelSaveController : ISaveController<LevelProgression>
    {
        private const string LEVEL_PROGRESSION_SAVE_FILE = "LevelProgressionSaveFile";
        
        public LevelProgression GetDefaultSaveFile()
        {
            return new LevelProgression()
            {
                HealthPoints = 100,
                HealthBarCount = 3,
                LastCheckPoint = default,
                EnemyPositions = new List<SerializedTransform>()
            };
        }
        
        public bool Push(in LevelProgression saveFile)
        {
            string json = JsonUtility.ToJson(saveFile);
            PlayerPrefs.SetString(LEVEL_PROGRESSION_SAVE_FILE, json);
            return true;
        }
        
        public bool Pull(out LevelProgression saveFile)
        {
            saveFile = default;
            if (!PlayerPrefs.HasKey(LEVEL_PROGRESSION_SAVE_FILE))
                return false;

            string json = PlayerPrefs.GetString(LEVEL_PROGRESSION_SAVE_FILE);
            saveFile = JsonUtility.FromJson<LevelProgression>(json);
            return true;
        }
    }
}