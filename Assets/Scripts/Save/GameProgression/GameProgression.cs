using System.Collections.Generic;
using SaveSystem.Core;
using UnityEngine;

namespace DeadLink.Save.GameProgression
{
    [System.Serializable]
    public struct GameProgression : ISaveFile
    {
        public int Version => 1;
        
        [SerializeField] public string DifficultyData;
        [SerializeField] public string PlayerName;
        
        [SerializeField] public int LevelIndex;
        
        [SerializeField] public List<string> RemainingPowerUps;
        [SerializeField] public List<string> PlayerPowerUps;
    }
}