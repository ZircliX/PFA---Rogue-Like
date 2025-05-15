using DeadLink.Level;
using SaveSystem.Core;

namespace DeadLink.Save.LevelProgression
{
    public class LevelScenarioSaveFileListener : ISaveListener<LevelScenarioSaveFile>
    {
        public int Priority => 1;

        public static LevelScenarioSaveFile CurrentLevelScenarioSaveFile { get; private set; } // Is valid ?
        public static ILevelManager CurrentILevelManager { get; set; }
        
        public void Write(ref LevelScenarioSaveFile saveFile)
        {
            if (CurrentILevelManager != null)
            {
                LevelScenarioSaveFile scenarioSaveFile = CurrentILevelManager.GetLevelScenario();

                saveFile.DifficultyData = scenarioSaveFile.DifficultyData;
                saveFile.LevelElements = scenarioSaveFile.LevelElements;

                CurrentLevelScenarioSaveFile = scenarioSaveFile;
            }
        }

        public void Read(in LevelScenarioSaveFile saveFile)
        {
            CurrentLevelScenarioSaveFile = saveFile;
        }
    }
}