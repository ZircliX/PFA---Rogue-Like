using System.Collections.Generic;
using System.Linq;
using DeadLink.Level.Interfaces;
using DeadLink.Misc;
using DevLocker.Utils;
using Enemy;
using RogueLike;
using RogueLike.Controllers;

namespace DeadLink.Level
{
    public struct LevelScenario
    {
        public DifficultyData DifficultyData;
        public SceneData Scene;
        public Dictionary<string, ILevelElementInfos> LevelElements;

        public LevelScenario(LevelScenarioSaveFile levelScenarioSaveFile)
        {
            DifficultyData = GameDatabase.Global.GetDifficulty(levelScenarioSaveFile.DifficultyData);
            Scene = GameDatabase.Global.GetScene(levelScenarioSaveFile.Scene);
            LevelElements = levelScenarioSaveFile.LevelElements.
                ToDictionary(ctx => ctx.GUID, ctx => ctx.ILevelElementInfos);
        }
        
        public static LevelScenario GetDefault()
        {
            return new LevelScenario()
            {
                DifficultyData = null,
                Scene = null,
                LevelElements = null
            };
        }
        
        public bool IsValid => DifficultyData != null && LevelElements != null;
    }
}