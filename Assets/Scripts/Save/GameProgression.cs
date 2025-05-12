using System.Collections.Generic;
using DeadLink.PowerUpSystem;
using Enemy;
using SaveSystem.Core;
using UnityEngine;

namespace RogueLike.Save
{
    [System.Serializable]
    public struct GameProgression : ISaveFile
    {
        public int Version => 1;
        
        public DifficultyData DifficultyData { get; set; }

        public int HealthPoints { get; set; }
        public int HealthBarCount { get; set; }
        
        public Transform LastCheckPoint { get; set; }
        public List<Transform> EnemyPositions { get; set; }
        
        public List<PowerUp> RemainingPowerUps { get; set; }
        public List<PowerUp> PlayerPowerUps { get; set; }
    }
}