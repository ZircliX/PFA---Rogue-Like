using System;
using DeadLink.Entities;
using Enemy;
using LTX.Singletons;
using RogueLike.Controllers;
using RogueLike.Player;
using RogueLike.Timer;
using UnityEngine;

namespace RogueLike.Managers
{
    public class LevelManager : MonoSingleton<LevelManager>
    {
        [field: SerializeField] public DifficultyData difficulty { get; private set; }
        [field: SerializeField] public Entities.Player player { get; private set; }
        [field: SerializeField] public PlayerMovement PlayerMovement { get; private set; }
        

        private void OnEnable()
        {
            WaveManager.Instance.OnAllEnemiesDie += EndWaveMode;
        }

        private void OnDisable()
        {
            WaveManager.Instance.OnAllEnemiesDie -= EndWaveMode;
        }

        protected void Start()
        {
            StartLevel();
        }
        
        private void StartLevel()
        {
            AudioManager.Global.PlayOneShot(GameMetrics.Global.FMOD_LevelStart, transform.position);
            
            if (GameMetrics.Global.SpawnEnemies)
                EnemyManager.Instance.SpawnEnemies(difficulty);
            player.Spawn(player.EntityData, difficulty, player.SpawnPosition.position);
            
            TimerManager.Instance.StartTimer();
        }

        public void StartWaveMode()
        {
            WaveManager.Instance.SetupWaveManager(difficulty);
        }

        public void EndWaveMode()
        {
            
        }

        public void FinishLevel()
        {
            TimerManager.Instance.PauseTimer();
            SceneController.Global.ChangeScene(GameMetrics.Global.ShopScene);
        }

        public void RetryLevel()
        {
            
        }
    }
}