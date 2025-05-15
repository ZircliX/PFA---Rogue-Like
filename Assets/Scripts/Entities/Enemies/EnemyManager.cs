using System.Collections.Generic;
using DeadLink.Entities.Data;
using DeadLink.Extensions;
using EditorAttributes;
using Enemy;
using LTX.Singletons;
using RogueLike.Controllers;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Pool;

namespace DeadLink.Entities
{
    public class EnemyManager : MonoSingleton<EnemyManager>
    {
        [SerializeField] private int wavesToSpawn;
        [SerializeField] private Transform[] SpawnPositions;
        public List<Enemy> SpawnedEnemies { get; private set; }
        
        [ButtonField(nameof(EditorSpawnEnemies), "SpawnEnemies")]
        [SerializeField] private Void RefreshWindows;

        public void EditorSpawnEnemies()
        {
            EntityData[] entityDatas = Resources.LoadAll<EntityData>("Entities/Enemies");
            for (int i = 0; i < SpawnPositions.Length; i++)
            {
                EntityData enemy = entityDatas[Random.Range(0, entityDatas.Length)];
                Object spawnedObject = PrefabUtility.InstantiatePrefab(enemy.EntityPrefab, SpawnPositions[i].transform.parent);
                
                if (spawnedObject is Entity entity)
                {
                    entity.transform.position = SpawnPositions[i].position;
                    entity.transform.rotation = SpawnPositions[i].rotation;
                }
            }

            EditorSceneManager.MarkSceneDirty(gameObject.scene);
        }
        
        protected override void Awake()
        {
            base.Awake();
            SpawnedEnemies = new List<Enemy>();
        }

        private void Update()
        {
            foreach (Enemy entity in SpawnedEnemies)
            {
                //Debug.Log(spawnedEnemies.Count);
                if (entity == null) continue;
                entity.OnUpdate();
            }
        }

        public Enemy SpawnEnemy(EntityData entity, DifficultyData difficultyData, SerializedTransform t)
        {
            Entity spawnedEnemy = Instantiate(entity.EntityPrefab, t.Position, t.Rotation.GetQuaternion());
            spawnedEnemy.Spawn(entity, difficultyData, t.Position);
            //Debug.Log($"Spawned {spawnedEnemy.name} at {t.Position}", spawnedEnemy);
            
            return spawnedEnemy as Enemy;
        }

        public void ClearEnemies()
        {
            using (ListPool<Enemy>.Get(out List<Enemy> enemies))
            {
                enemies.AddRange(SpawnedEnemies);
                
                foreach (Enemy enemy in enemies)
                {
                    enemy.Dispose();
                }
            }
            
            SpawnedEnemies.Clear();
        }
        
        public void RegisterEnemy(Enemy enemy)
        {
            SpawnedEnemies.Add(enemy);
        }
        
        public void UnregisterEnemy(Enemy enemy)
        {
            SpawnedEnemies.Remove(enemy);
        }
        
        public void EnemyKilled(Enemy enemy)
        {
            //Imobiliser le mob
            enemy.EntityData.VFXToSpawn.PlayVFX(enemy.transform.position, enemy.EntityData.DelayAfterDestroyVFX);
            Debug.Log("Enemy Killed");
            SpawnedEnemies.Remove(enemy);
            Destroy(enemy.gameObject, 2f);
        }
    }
}