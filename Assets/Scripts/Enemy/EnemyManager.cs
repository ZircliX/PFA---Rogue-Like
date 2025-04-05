using System;
using LTX.Singletons;
using RogueLike.Controllers;
using UnityEngine;

namespace Enemy
{
    public class EnemyManager : MonoSingleton<EnemyManager>
    {
        [SerializeField] private Enemy[] enemyPrefabs;
        [SerializeField] private Transform[] SpawnPositions;

        public event Action OnAllEnemiesDie;
        public int RemainingsEnemies { get; private set; }
        public bool AllEnemiesKilled => RemainingsEnemies == 0;
        
        public void SpawnEnemies(DifficultyData difficultyData)
        {
            //Debug.Log("Spawn enemies");
            for (var i = 0; i < enemyPrefabs.Length; i++)
            {
                Enemy spawnedEnemy = Instantiate(enemyPrefabs[i], SpawnPositions[i].position, Quaternion.identity);
                spawnedEnemy.Spawn(spawnedEnemy.CurrentData, difficultyData , SpawnPositions[i].position);

                RemainingsEnemies++;
                // pas sur pour le spawnPosition
            }
        }
        
        public void EnemyKilled(Enemy EnemyKilled)
        {
            //Imobiliser le mob
            GameController.VFXManager.VFX(EnemyKilled.VFXToSpawn, EnemyKilled.transform.position, EnemyKilled.DelayAfterDestroyVFX);
            RemainingsEnemies--;
            
            if (AllEnemiesKilled)
            {
                OnAllEnemiesDie?.Invoke();
            }
        }
    }
}