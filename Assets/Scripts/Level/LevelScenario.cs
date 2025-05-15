using System.Collections.Generic;
using System.Linq;
using DeadLink.Level.Interfaces;
using Enemy;
using RogueLike.Controllers;

namespace DeadLink.Level
{
    public struct LevelScenario
    {
        public DifficultyData DifficultyData;
        public Dictionary<string, ILevelElementInfos> LevelElements;

        public LevelScenario(LevelScenarioSaveFile levelScenarioSaveFile)
        {
            DifficultyData = GameDatabase.Global.GetDifficulty(levelScenarioSaveFile.DifficultyData);
            LevelElements = levelScenarioSaveFile.LevelElements.
                ToDictionary(ctx => ctx.GUID, ctx => ctx.ILevelElementInfos);
        }
    }
}