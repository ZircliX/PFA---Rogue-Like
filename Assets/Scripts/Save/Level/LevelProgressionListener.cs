using System.Collections.Generic;
using DeadLink.Extensions;
using SaveSystem.Core;
using UnityEngine;
using ZLinq;

namespace DeadLink.Save.Level
{
    public class LevelProgressionListener : ISaveListener<LevelProgression>
    {
        public int Priority => 1;
        
        public int HealthPoints { get; private set; }
        public int HealthBarCount { get; private set; }
        
        public Transform LastCheckPoint { get; private set; }
        public List<Transform> EnemyPositions { get; private set; }
        
        public void Write(ref LevelProgression saveFile)
        {
            saveFile.HealthPoints = HealthPoints;
            saveFile.HealthBarCount = HealthBarCount;
            
            saveFile.LastCheckPoint = LastCheckPoint.ToSerializeTransform();
            saveFile.EnemyPositions = EnemyPositions.AsValueEnumerable().Select(transform => transform.ToSerializeTransform()).ToList();
        }

        public void Read(in LevelProgression saveFile)
        {
            HealthPoints = saveFile.HealthPoints;
            HealthBarCount = saveFile.HealthBarCount;
            
            //LastCheckPoint = saveFile.LastCheckPoint;
            //EnemyPositions = saveFile.EnemyPositions;
        }
        
        public void SetHealthPoints(int healthPoints) => HealthPoints = healthPoints;

        public void SetHealthBarCount(int healthBarCount) => HealthBarCount = healthBarCount;

        public void SetLastCheckPoint(Transform lastCheckPoint) => LastCheckPoint = lastCheckPoint;

        public void SetEnemyPositions(List<Transform> enemyPositions) => EnemyPositions = enemyPositions;
    }
}