using System.Collections.Generic;
using DeadLink.Extensions;
using SaveSystem.Core;
using UnityEngine;

namespace DeadLink.Save.Level
{
    [System.Serializable]
    public struct LevelProgression : ISaveFile
    {
        public int Version => 1;
        
        [SerializeField] public int HealthPoints;
        [SerializeField] public int HealthBarCount;
        
        [SerializeField] public SerializedTransform LastCheckPoint;
        [SerializeField] public List<SerializedTransform> EnemyPositions;
    }
}