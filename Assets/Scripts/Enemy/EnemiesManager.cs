using System;
using KBCore.Refs;
using LTX.Singletons;
using RogueLike.Controllers;
using UnityEngine;

namespace Enemy
{
    public class EnemiesManager : MonoSingleton<EnemiesManager>
    {
        [SerializeField, Scene] private Enemy[] enemies;
        [SerializeField] private Vector3[] SpawnPositions;

        public event Action OnAllEnemiesDie;
        public int RemainingsEnemies { get; private set; }
        public bool AllEnemiesKilled => RemainingsEnemies == 0;

        private void OnValidate() => this.ValidateRefs();

        public void SpawnEnemies(DifficultyData difficultyData)
        {
            for (var i = 0; i < enemies.Length; i++)
            {
                Enemy enemiesToSpawn = enemies[i];
                enemiesToSpawn.Spawn(enemiesToSpawn.CurrentData, difficultyData , SpawnPositions[i]);

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