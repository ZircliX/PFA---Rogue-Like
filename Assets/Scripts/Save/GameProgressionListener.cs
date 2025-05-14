using System.Collections.Generic;
using DeadLink.Extensions;
using DeadLink.PowerUpSystem;
using Enemy;
using SaveSystem.Core;
using UnityEngine;
using ZLinq;

namespace RogueLike.Save
{
    public class GameProgressionListener : ISaveListener<GameProgression>
    {
        public int Priority => 1;
        
        public DifficultyData DifficultyData { get; private set; }
        public string PlayerName { get; private set; }

        public int HealthPoints { get; private set; }
        public int HealthBarCount { get; private set; }
        
        public Transform LastCheckPoint { get; private set; }
        public List<Transform> EnemyPositions { get; private set; }
        
        public List<PowerUp> RemainingPowerUps { get; private set; }
        public List<PowerUp> PlayerPowerUps { get; private set; }

        #region Write / Read
        
        public void Write(ref GameProgression saveFile)
        {
            saveFile.DifficultyData = DifficultyData.GUID;
            saveFile.PlayerName = PlayerName;
            
            saveFile.HealthPoints = HealthPoints;
            saveFile.HealthBarCount = HealthBarCount;
            
            saveFile.LastCheckPoint = LastCheckPoint.ToSerializeTransform();
            saveFile.EnemyPositions = EnemyPositions.AsValueEnumerable().Select(transform => transform.ToSerializeTransform()).ToList();
            
            saveFile.RemainingPowerUps = RemainingPowerUps.AsValueEnumerable().Select(up => up.GUID).ToList();
            saveFile.PlayerPowerUps = PlayerPowerUps.AsValueEnumerable().Select(up => up.GUID).ToList();
        }

        public void Read(in GameProgression saveFile)
        {
            //DifficultyData = saveFile.DifficultyData;
            PlayerName = saveFile.PlayerName;
            
            HealthPoints = saveFile.HealthPoints;
            HealthBarCount = saveFile.HealthBarCount;
            
            //LastCheckPoint = saveFile.LastCheckPoint;
            //EnemyPositions = saveFile.EnemyPositions;
            
            //RemainingPowerUps = saveFile.RemainingPowerUps;
            //PlayerPowerUps = saveFile.PlayerPowerUps;
        }
        
        #endregion
        
        public void SetDifficultyData(DifficultyData data)
        {
            DifficultyData = data;
        }
        
        public void SetPlayerName(string playerName)
        {
            PlayerName = playerName;
        }
        
        public void SetHealthPoints(int healthPoints)
        {
            HealthPoints = healthPoints;
        }
        
        public void SetHealthBarCount(int healthBarCount)
        {
            HealthBarCount = healthBarCount;
        }
        
        public void SetLastCheckPoint(Transform lastCheckPoint)
        {
            LastCheckPoint = lastCheckPoint;
        }
        
        public void SetEnemyPositions(List<Transform> enemyPositions)
        {
            EnemyPositions = enemyPositions;
        }
        
        public void SetRemainingPowerUps(List<PowerUp> remainingPowerUps)
        {
            RemainingPowerUps = remainingPowerUps;
        }

        public void SetPlayerPowerUps(List<PowerUp> playerPowerUps)
        {
            PlayerPowerUps = playerPowerUps;
        }
    }
}