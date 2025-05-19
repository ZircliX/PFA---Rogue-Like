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
        public bool UseCustomInfos;
        public Dictionary<string, ILevelElementInfos> LevelElementsCustomInfos;

        public LevelScenario(LevelScenarioSaveFile levelScenarioSaveFile)
        {
            DifficultyData = GameDatabase.Global.GetDifficulty(levelScenarioSaveFile.DifficultyData);
            Scene = GameDatabase.Global.GetScene(levelScenarioSaveFile.Scene);
            UseCustomInfos = true;
            LevelElementsCustomInfos = levelScenarioSaveFile.LevelElements.
                ToDictionary(ctx => ctx.GUID, ctx => ctx.ILevelElementInfos);
        }
        
        public static LevelScenario GetDefault()
        {
            return new LevelScenario()
            {
                DifficultyData = null,
                Scene = null,
                LevelElementsCustomInfos = null,
                UseCustomInfos = false
            };
        }
        
        public bool IsValid => DifficultyData != null && (!UseCustomInfos || LevelElementsCustomInfos != null);
    }
}