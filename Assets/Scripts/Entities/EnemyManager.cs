using Enemy;
using KBCore.Refs;
using LTX.Singletons;
using RogueLike.Controllers;
using RogueLike.Managers;
using UnityEngine;

namespace DeadLink.Entities
{
    public class EnemyManager : MonoSingleton<EnemyManager>
    {
        [SerializeField] private Enemy[] enemyPrefabs;
        [SerializeField] private int numberOfEnemiesToSpawn;
        [SerializeField] private Transform[] SpawnPositions;

        [SerializeField, Scene] private Enemy[] enemies;
        
        private void OnValidate() => this.ValidateRefs();

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                SpawnEnemies(LevelManager.Instance.difficulty);
            }
        }
        
        public void SpawnEnemies(DifficultyData difficultyData)
        {
            if (enemyPrefabs == null) return;
            for (var i = 0; i < numberOfEnemiesToSpawn; i++)
            {
                Vector3 position = SpawnPositions[Random.Range(0, SpawnPositions.Length)].position;
                Enemy spawnedEnemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], position, Quaternion.identity);
                spawnedEnemy.Spawn(spawnedEnemy.EntityData, difficultyData, position);
            }
        }
        
        public void ActivateEnemies(DifficultyData difficulty)
        {
            if (enemies == null) return;
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