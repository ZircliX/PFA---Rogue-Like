using System.Collections;
using System.Collections.Generic;
using DeadLink.Entities;
using DeadLink.Level;
using DeadLink.Level.Interfaces;
using DeadLink.Menus;
using DeadLink.Player;
using DeadLink.Save.LevelProgression;
using DeadLink.SceneManagement;
using Enemy;
using LTX.ChanneledProperties;
using LTX.Singletons;
using RogueLike.Timer;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZLinq;

namespace RogueLike.Managers
{
    public class LevelManager : MonoSingleton<LevelManager>, ILevelManager
    {
        public class DefaultScenario : ILevelScenarioProvider
        {
            public LevelScenario GetLevelScenario(LevelManager levelManager)
            {
                return new LevelScenario()
                {
                    DifficultyData = GameMetrics.Global.NormalDifficulty,
                    LevelElements = levelManager.LevelElements
                        .AsValueEnumerable()
                        .ToDictionary(ctx => ctx.GUID, ctx => ctx.Pull())
                };
            }
        }
        
        [field: SerializeField] public PlayerController PlayerController { get; private set; }

        public DifficultyData Difficulty { get; private set; }
        
        public PrioritisedProperty<ILevelScenarioProvider> LevelScenarioProvider { get; private set; }
        public List<LevelElement> LevelElements { get; private set; }
        
        private void OnEnable()
        {
            WaveManager.Instance.OnAllEnemiesDie += EndWaveMode;
            LevelScenarioSaveFileListener.CurrentILevelManager = this;
        }

        private void OnDisable()
        {
            WaveManager.Instance.OnAllEnemiesDie -= EndWaveMode;
            LevelScenarioSaveFileListener.CurrentILevelManager = null;
        }

        protected override void Awake()
        {
            base.Awake();
            LevelScenarioProvider = new PrioritisedProperty<ILevelScenarioProvider>(new DefaultScenario());
            LevelElements = new List<LevelElement>();
        }

        protected IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            
            PrepareLevel(LevelScenarioProvider.Value.GetLevelScenario(this));
            StartLevel();
        }
        
        private void PrepareLevel(LevelScenario levelScenario)
        {
            Difficulty = levelScenario.DifficultyData;

            foreach (LevelElement element in LevelElements)
            {
                foreach (KeyValuePair<string, ILevelElementInfos> elementSaveFile in levelScenario.LevelElements)
                {
                    if (element.GUID != elementSaveFile.Key) continue;
                    
                    element.Push(elementSaveFile.Value);
                }
            }
                
            if (GameMetrics.Global.SpawnEnemies)
                EnemyManager.Instance.SpawnEnemies(Difficulty);

            PlayerController.PlayerEntity.Spawn(
                PlayerController.PlayerEntity.EntityData, 
                Difficulty, 
                PlayerController.PlayerEntity.SpawnPosition.position);
        }

        public void StartLevel()
        {
            TimerManager.Instance.StartTimer();
            AudioManager.Global.PlayOneShot(GameMetrics.Global.FMOD_LevelStart, transform.position);
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

        public LevelScenarioSaveFile GetLevelScenario()
        {
            return new LevelScenarioSaveFile()
            {
                DifficultyData = Difficulty.GUID,
                LevelElements = LevelElements
                    .AsValueEnumerable()
                    .Select(ctx => new LevelScenarioSaveFile.LevelElementSaveFile()
                    {
                        GUID = ctx.GUID,
                        ILevelElementInfos = ctx.Pull()
                    }).ToList()
            };
        }
    }
}