using System;
using System.Collections.Generic;
using DeadLink.Entities.Data;
using DeadLink.Extensions;
using EditorAttributes;
using Enemy;
using LTX.ChanneledProperties;
using LTX.Singletons;
using RogueLike.Controllers;
using RogueLike.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DeadLink.Entities
{
    public class WaveManager : MonoSingleton<WaveManager>
    {
        [field: SerializeField] public InfluencedProperty<int> WaveBalance { get; private set; }
        [field: SerializeField] public InfluencedProperty<int> WaveCount { get; private set; }
        [SerializeField, ReadOnly] private int currentWaveBalance;
        [SerializeField, ReadOnly] private int currentWaveCount;
        [field: SerializeField] public List<Transform> WaveSpawnPoints { get; private set; }
        private EnemyData enemyData;
        public bool WaveMode { get; private set; }

        public int RemainingEnemies { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            DifficultyData difficultyData = LevelManager.Instance.Difficulty;
            SetupWaveManager(difficultyData);
        }

        public void SetupWaveManager(DifficultyData difficulty)
        {
            WaveBalance = new InfluencedProperty<int>(difficulty.BaseWaveBalance);
            WaveCount = new InfluencedProperty<int>(difficulty.BaseWaveCount);
            WaveBalance.AddInfluence(difficulty, difficulty.BaseWaveBalance, Influence.Multiply);
            WaveCount.AddInfluence(difficulty, difficulty.BaseWaveCount, Influence.Add);
            currentWaveBalance = WaveBalance.Value;
            currentWaveCount = WaveCount.Value;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                StartWaveMode();
            }
        }

        private void StartWaveMode()
        {
            EntityData enemy = GameDatabase.Global.EnemiesDatas[1];
            if (enemy is EnemyData ed)
            {
                enemyData = ed;
            }
            
            UpdateWaveMode();
        }

        private void UpdateWaveMode()
        {
            if (currentWaveBalance - enemyData.Cost <= 0)
            {
                currentWaveBalance = 0;
                RemainingEnemies = EnemyManager.Instance.SpawnedEnemies.Count;
                return;
            }
            InstantiateEnemy();
            UpdateWaveMode();
        }

        private void InstantiateEnemy()
        {
            EnemyManager.Instance.SpawnEnemy(enemyData,LevelManager.Instance.Difficulty, WaveSpawnPoints[Random.Range(0,WaveSpawnPoints.Count)].ToSerializeTransform());
            currentWaveBalance -= enemyData.Cost;
        }

        private void ResetWaveBalance()
        {
            currentWaveBalance = WaveBalance.Value;
            UpdateWaveMode();
        }

        private void OnWaveModeFinished()
        {
            Debug.Log("c'est finito");
            //Active portal to next level !
        }

        public void OnWaveFinished()
        {
            if (currentWaveCount - 1 <= 0)
            {
                currentWaveCount--;
                OnWaveModeFinished();
                return;
            }
            currentWaveCount--;
            Debug.Log($"Starting next wave and count {currentWaveCount}");
            ResetWaveBalance();

        }
    }
}