using DeadLink.Entities.Data;
using DeadLink.PowerUpSystem;
using Enemy;
using UnityEngine;

namespace RogueLike.Controllers
{
    public class GameDatabase
    {
        public static GameDatabase Global => GameController.GameDatabase;

        #region Difficulties

        public DifficultyData[] Difficulties { get; private set; }
        
        public DifficultyData GetDifficulty(string targetGUID)
        {
            for (int index = 0; index < Difficulties.Length; index++)
            {
                DifficultyData difficultyData = Difficulties[index];
                if (difficultyData.GUID == targetGUID)
                {
                    return difficultyData;
                }
            }

            return null;
        }

        #endregion
        
        #region EntityDatas

        public EntityData PlayerEntityData { get; private set; }
        public EntityData[] EnemiesDatas { get; private set; }
        
        public EntityData GetEnemyData(string targetGUID)
        {
            for (int index = 0; index < EnemiesDatas.Length; index++)
            {
                EntityData enemyData = EnemiesDatas[index];
                if (enemyData.GUID == targetGUID)
                {
                    return enemyData;
                }
            }

            return null;
        }

        #endregion
        
        #region PowerUps
        public PowerUp[] PowerUps { get; private set; }

        public PowerUp GetPowerUp(string targetName)
        {
            for (int index = 0; index < PowerUps.Length; index++)
            {
                PowerUp powerUp = PowerUps[index];
                if (powerUp.Name == targetName)
                {
                    return powerUp;
                }
            }

            return null;
        }
        
        #endregion

        public void Load()
        {
            PowerUps = Resources.LoadAll<PowerUp>("PowerUps");
            EnemiesDatas = Resources.LoadAll<EntityData>("Entities/Enemies");
            PlayerEntityData = Resources.Load<EntityData>("Entities/Player");
            
            Difficulties = new[]
            {
                GameMetrics.Global.EasyDifficulty,
                GameMetrics.Global.NormalDifficulty,
                GameMetrics.Global.HardDifficulty,
                GameMetrics.Global.InsaneDifficulty
            };
        }
    }
}