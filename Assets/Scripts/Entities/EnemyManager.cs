using System.Collections.Generic;
using Enemy;
using LTX.Singletons;
using RogueLike.Controllers;
using RogueLike.Managers;
using UnityEngine;

namespace DeadLink.Entities
{
    public class EnemyManager : MonoSingleton<EnemyManager>
    {
        [SerializeField] private Entity[] enemyPrefabs;
        [SerializeField] private int wavesToSpawn;
        [SerializeField] private Transform[] SpawnPositions;
        private List<Entity> spawnedEnemies;

        protected override void Awake()
        {
            base.Awake();
            spawnedEnemies = new List<Entity>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                SpawnEnemies(LevelManager.Instance.Difficulty);
            }

            foreach (Entity entity in spawnedEnemies)
            {
                entity.OnFixedUpdate();
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
                    Entity spawnedEnemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], position, Quaternion.identity);
                    spawnedEnemy.Spawn(spawnedEnemy.EntityData, difficultyData, position);
                    
                    spawnedEnemies.Add(spawnedEnemy);
                }
            }
        }
        
        public void EnemyKilled(Entity entity)
        {
            //Imobiliser le mob
            entity.EntityData.VFXToSpawn.PlayVFX(entity.transform.position, entity.EntityData.DelayAfterDestroyVFX);
            spawnedEnemies.Remove(entity);
            Destroy(entity.gameObject, 2f);
        }
    }
}