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
                Scene = string.Empty,
                LevelElements = null,
            };
        }
        
        public int Version => 1;
        public bool IsValid => !string.IsNullOrEmpty(DifficultyData) && !string.IsNullOrEmpty(Scene) && LevelElements != null;

        [SerializeField] public string DifficultyData;
        [SerializeField] public string Scene;
        [SerializeField] public List<LevelElementSaveFile> LevelElements;

        public LevelScenarioSaveFile(LevelScenario levelScenario)
        {
            //Debug.Log("convert to save file");
            DifficultyData = levelScenario.DifficultyData.GUID;
            Scene = levelScenario.Scene.GUID;
            LevelElements = levelScenario.LevelElementsCustomInfos.Select(kvp => new LevelElementSaveFile()
            {
                GUID = kvp.Key,
                ILevelElementInfos = kvp.Value
            }).ToList();
        }
        
        public override string ToString()
        {
            return $"Difficulty : {DifficultyData}, Scene : {Scene}, LevelElement : {LevelElements.Count}, IsValid : {IsValid}";
        }
    }
}