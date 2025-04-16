using DeadLink.Entities;
using Enemy;
using LTX.Singletons;
using RogueLike.Controllers;
using RogueLike.Timer;
using UnityEngine;

namespace RogueLike.Managers
{
    public class LevelManager : MonoSingleton<LevelManager>
    {
        [SerializeField] private DifficultyData difficulty;
        [SerializeField] private Entities.Player player;

        private void OnEnable()
        {
            WaveManager.Instance.OnAllEnemiesDie += EndWaveMode;
        }

        private void OnDisable()
        {
            WaveManager.Instance.OnAllEnemiesDie -= EndWaveMode;
        }

        private void Start()
        {
            StartLevel();
        }
        
        public void StartLevel()
        {
            //Debug.Log("Start Level");
            //EnemyManager.Instance.SpawnEnemies(difficulty);
            //EnemyManager.Instance.ActivateEnemies(difficulty); //Only for testing
            player.Spawn(player.EntityData, difficulty, player.SpawnPosition.position);
            
            TimerManager.Instance.StartTimer();
            // PlayerManager qui fait spawn le player? Ou c'est le LevelManager Qui fait spawn Le joueur ?
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
    }
}