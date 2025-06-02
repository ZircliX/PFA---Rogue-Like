using System.Collections.Generic;
using DeadLink.Entities.Data;
using DeadLink.Extensions;
using DG.Tweening;
using EditorAttributes;
using Enemy;
using LTX.Singletons;
using RogueLike.Controllers;
using UnityEngine;
using UnityEngine.Pool;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace DeadLink.Entities
{
    public class EnemyManager : MonoSingleton<EnemyManager>
    {
        [SerializeField] private Transform[] SpawnPositions;
        public List<Enemy> SpawnedEnemies { get; private set; }


#if UNITY_EDITOR
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
#endif
        
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
            if (SpawnedEnemies.Count - 1 <= 0)
            {
                SpawnedEnemies.Remove(enemy);
                WaveManager.Instance.OnWaveFinished();
            }
            SpawnedEnemies.Remove(enemy);
        }
        
        public void EnemyKilled(Enemy enemy)
        {
            SpawnedEnemies.Remove(enemy);
            Destroy(enemy.gameObject);
        }
    }
}