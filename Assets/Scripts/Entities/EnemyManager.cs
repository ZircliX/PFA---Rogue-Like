using Enemy;
using LTX.Singletons;
using RogueLike.Controllers;
using RogueLike.Managers;
using UnityEngine;

namespace DeadLink.Entities
{
    public class EnemyManager : MonoSingleton<EnemyManager>
    {
        [SerializeField] private Enemy[] enemyPrefabs;
        [SerializeField] private int wavesToSpawn;
        [SerializeField] private Transform[] SpawnPositions;

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
            for (var i = 0; i < wavesToSpawn; i++)
            {
                for (int j = 0; j < SpawnPositions.Length; j++)
                {
                    Vector3 position = SpawnPositions[j].position;
                    Enemy spawnedEnemy = Instantiate(enemyPrefabs[0], position, Quaternion.identity);
                    spawnedEnemy.Spawn(spawnedEnemy.EntityData, difficultyData, position);
                }
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