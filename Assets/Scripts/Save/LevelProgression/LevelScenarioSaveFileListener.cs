using DeadLink.Level;
using Enemy;
using RogueLike.Controllers;
using SaveSystem.Core;
using UnityEngine;

namespace DeadLink.Save.LevelProgression
{
    public class LevelScenarioSaveFileListener : ISaveListener<LevelScenarioSaveFile>
    {
        public int Priority => 1;

        public static LevelScenarioSaveFile CurrentLevelScenarioSaveFile { get; private set; }
        public static ILevelManager CurrentILevelManager { get; set; }
        
        public void Write(ref LevelScenarioSaveFile saveFile)
        {
            if (CurrentILevelManager != null)
            {
                LevelScenarioSaveFile scenarioSaveFile = CurrentILevelManager.GetLevelScenario();
                //Debug.Log("writing save " + scenarioSaveFile);
                if (!scenarioSaveFile.IsValid) return;
                
                CurrentLevelScenarioSaveFile = scenarioSaveFile;

                if (!scenarioSaveFile.IsValid) return;
                
                saveFile.DifficultyData = scenarioSaveFile.DifficultyData;
                saveFile.Scene = scenarioSaveFile.Scene;
                saveFile.LevelElements = scenarioSaveFile.LevelElements;
            }
        }

        public void Read(in LevelScenarioSaveFile saveFile)
        {
            CurrentLevelScenarioSaveFile = saveFile;
        }
    }
}