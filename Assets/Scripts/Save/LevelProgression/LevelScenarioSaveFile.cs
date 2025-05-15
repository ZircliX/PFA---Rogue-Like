using System.Collections.Generic;
using System.Linq;
using DeadLink.Level.Interfaces;
using SaveSystem.Core;
using UnityEngine;

namespace DeadLink.Level
{
    [System.Serializable]
    public struct LevelScenarioSaveFile : ISaveFile
    {
        [System.Serializable]
        public struct LevelElementSaveFile
        {
            [SerializeField] public string GUID;
            [SerializeReference] public ILevelElementInfos ILevelElementInfos;
        }
        
        public static LevelScenarioSaveFile GetDefault()
        {
            return new LevelScenarioSaveFile()
            {
                DifficultyData = string.Empty,
                LevelElements = null,
            };
        }
        
        public int Version => 1;
        public bool IsValid => !string.IsNullOrEmpty(DifficultyData) && LevelElements != null;

        [SerializeField] public string DifficultyData;
        [SerializeField] public List<LevelElementSaveFile> LevelElements;

        public LevelScenarioSaveFile(LevelScenario levelScenario)
        {
            DifficultyData = levelScenario.DifficultyData.GUID;
            LevelElements = levelScenario.LevelElements.Select(kvp => new LevelElementSaveFile()
            {
                GUID = kvp.Key,
                ILevelElementInfos = kvp.Value
            }).ToList();
        }
    }
}