using System;
using Enemy;
using KBCore.Refs;
using LTX.Singletons;
using RogueLike.Controllers;
using RogueLike.Managers;
using UnityEngine;

namespace Enemy
{
    public class EnemiesManager : MonoSingleton<EnemiesManager>
    {
        [SerializeField, Scene] private Enemy[] enemies;
        [SerializeField] private Vector3[] SpawnPositions;

        public event Action OnAllEnemiesDie;
        
        private int RemainingsEnemies;

        public void SpawnEnemies(DifficultyData difficultyData)
        {
            for (var i = 0; i < enemies.Length; i++)
            {
                var enemiesToSpawn = enemies[i];
                enemiesToSpawn.Spawn(enemiesToSpawn.CurrentData, difficultyData , SpawnPositions[i]);
                // pas sur pour le spawnPosition
            }
        }
        
        public void EnemyKilled(Enemy EnemyKilled)
        {
            //Imobiliser le mob
            GameController.VfxManager.VFX(EnemyKilled.vfxToSpawn, EnemyKilled.transform.position, EnemyKilled.delayAfterDestroyVfx);
            
            if (AllEnemyKilled())
            {
                OnAllEnemiesDie?.Invoke();
            }
        }

        public bool AllEnemyKilled()
        {
            return RemainingsEnemies == 0;
        }
    }
}