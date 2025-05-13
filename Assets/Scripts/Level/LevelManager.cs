using System;
using DeadLink.Entities;
using DeadLink.Menus;
using Enemy;
using LTX.Singletons;
using RogueLike.Controllers;
using RogueLike.Player;
using RogueLike.Timer;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RogueLike.Managers
{
    public class LevelManager : MonoSingleton<LevelManager>
    {
        [field: SerializeField] public DifficultyData Difficulty { get; private set; }
        [field: SerializeField] public Entities.Player Player { get; private set; }
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
                EnemyManager.Instance.SpawnEnemies(Difficulty);

            Player.Spawn(Player.EntityData, Difficulty, Player.SpawnPosition.position);
            
            TimerManager.Instance.StartTimer();
        }

        public void StartWaveMode()
        {
            WaveManager.Instance.SetupWaveManager(Difficulty);
        }

        public void EndWaveMode()
        {
            
        }

        public void FinishLevel()
        {
            TimerManager.Instance.PauseTimer();
            
            IMenu menu = MenuManager.Instance.GetMenu(GameMetrics.Global.ScoreboardMenu);
            MenuManager.Instance.OpenMenu(menu);
            //SceneController.Global.ChangeScene(GameMetrics.Global.ShopScene.BuildIndex);
        }

        public void RetryLevel()
        {
            SceneController.Global.ChangeScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}