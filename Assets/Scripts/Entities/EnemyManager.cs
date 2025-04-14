using Enemy;
using KBCore.Refs;
using LTX.Singletons;
using RogueLike.Controllers;
using UnityEngine;

namespace DeadLink.Entities
{
    public class EnemyManager : MonoSingleton<EnemyManager>
    {
        [SerializeField] private Enemy[] enemyPrefabs;
        [SerializeField] private Transform[] SpawnPositions;

        [SerializeField, Scene] private Enemy[] enemies;
        
        private void OnValidate() => this.ValidateRefs();
        
        public void SpawnEnemies(DifficultyData difficultyData)
        {
            for (var i = 0; i < enemyPrefabs.Length; i++)
            {
                Enemy spawnedEnemy = Instantiate(enemyPrefabs[i], SpawnPositions[i].position, Quaternion.identity);
                spawnedEnemy.Spawn(spawnedEnemy.EntityData, difficultyData , SpawnPositions[i].position);
            }
        }
        
        public void ActivateEnemies(DifficultyData difficulty)
        {
            for (var i = 0; i < enemies.Length; i++)
            {
                Enemy enemy = enemies[i];
                enemy.Spawn(enemy.EntityData, difficulty , transform.position);
            }
        }
        
        public void EnemyKilled(Enemy enemyKilled)
        {
            //Imobiliser le mob
            GameController.VFXManager.VFX(enemyKilled.EntityData.VFXToSpawn, enemyKilled.transform.position, enemyKilled.EntityData.DelayAfterDestroyVFX);
            Destroy(enemyKilled.gameObject, 2f);
        }
    }
}