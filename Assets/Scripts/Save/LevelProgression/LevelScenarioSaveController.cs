using DeadLink.Level;
using SaveSystem.Core;
using UnityEngine;

namespace DeadLink.Save.LevelProgression
{
    public class LevelScenarioSaveController : ISaveController<LevelScenarioSaveFile>
    {
        private const string LEVEL_SCENARIO_SAVE_FILE = "LevelScenarioSaveFile";
        
        public LevelScenarioSaveFile GetDefaultSaveFile()
        {
            return LevelScenarioSaveFile.GetDefault();
        }
        
        public bool Push(in LevelScenarioSaveFile saveFile)
        {
            string json = JsonUtility.ToJson(saveFile);
            PlayerPrefs.SetString(LEVEL_SCENARIO_SAVE_FILE, json);
            return true;
        }
        
        public bool Pull(out LevelScenarioSaveFile saveFile)
        {
            saveFile = default;
            if (!PlayerPrefs.HasKey(LEVEL_SCENARIO_SAVE_FILE))
                return false;

            string json = PlayerPrefs.GetString(LEVEL_SCENARIO_SAVE_FILE);
            saveFile = JsonUtility.FromJson<LevelScenarioSaveFile>(json);

            return saveFile.IsValid;
        }
    }
}