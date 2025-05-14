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
        
        [SerializeField] public string DifficultyData;
        [SerializeField] public string PlayerName;

        [SerializeField] public int HealthPoints;
        [SerializeField] public int HealthBarCount;
        
        [SerializeField] public SerializedTransform LastCheckPoint;
        [SerializeField] public List<SerializedTransform> EnemyPositions;
        
        [SerializeField] public List<string> RemainingPowerUps;
        [SerializeField] public List<string> PlayerPowerUps;
    }

    [System.Serializable]
    public struct SerializedTransform
    {
        public Vector3 Position;
        public Vector4 Rotation;
        public Vector3 Scale;
    }
}