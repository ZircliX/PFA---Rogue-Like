using System.Collections;
using System.Collections.Generic;
using DeadLink.Entities;
using DeadLink.Level;
using DeadLink.Level.Interfaces;
using DeadLink.Menus;
using DeadLink.Misc;
using DeadLink.Player;
using DeadLink.Save.LevelProgression;
using DeadLink.SceneManagement;
using Enemy;
using LTX.ChanneledProperties;
using LTX.Singletons;
using RogueLike.Controllers;
using RogueLike.Timer;
using SaveSystem.Core;
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
                    Scene = GameDatabase.Global.GetSceneDataFromScene(levelManager.gameObject.scene, out SceneData data) ? data : GameDatabase.Global.Scenes[0],
                    LevelElementsCustomInfos = levelManager.LevelElements
                        .AsValueEnumerable()
                        .ToDictionary(ctx => ctx.GUID, ctx => ctx.Pull())
                };
            }
        }
        
        [field: SerializeField] public PlayerController PlayerController { get; private set; }

        public DifficultyData Difficulty { get; private set; }
        
        public PrioritisedProperty<ILevelScenarioProvider> LevelScenarioProvider { get; private set; }
        public LevelScenario LastLevelScenario { get; private set; }
        public List<LevelElement> LevelElements { get; private set; }
        
        private void OnEnable()
        {
            //WaveManager.Instance.OnAllEnemiesDie += EndWaveMode;
            LevelScenarioSaveFileListener.CurrentILevelManager = this;
        }

        private void OnDisable()
        {
            //WaveManager.Instance.OnAllEnemiesDie -= EndWaveMode;
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
            
            //Debug.Log("Start");
            LastLevelScenario = LevelScenarioProvider.Value.GetLevelScenario(this);
            PrepareLevel();
            StartLevel();
        }
        
        private void PrepareLevel()
        {
            Difficulty = LastLevelScenario.DifficultyData;
            var infosMap = LastLevelScenario.UseCustomInfos ? 
                LastLevelScenario.LevelElementsCustomInfos :
                GetCurrentElementInfos();
            
            foreach (LevelElement element in LevelElements)
            {
                foreach (KeyValuePair<string, ILevelElementInfos> elementSaveFile in infosMap)
                {
                    if (element.GUID != elementSaveFile.Key) continue;

                    element.Push(elementSaveFile.Value);
                }
            }
        }

        public void StartLevel()
        {
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
            //LastLevelScenario = LevelScenario.GetDefault();
            //PlayerController.LastCheckPoint = null;
            SaveManager<LevelScenarioSaveFile>.Push();
            
            IMenu menu = MenuManager.Instance.GetMenu(GameMetrics.Global.GameplayScoreboard);
            MenuManager.Instance.OpenMenu(menu);
        }

        public void LeaveLevel()
        {
            TimerManager.Instance.PauseTimer();
            SaveManager<LevelScenarioSaveFile>.Push();
            
            SceneController.Global.ChangeScene(GameMetrics.Global.MainMenuScene);
        }

        public void RetryLevel()
        {
            SceneController.Global.ChangeScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void ReloadFromLastScenario()
        {
            PrepareLevel();
            StartLevel();
        }

        LevelScenarioSaveFile ILevelManager.GetLevelScenario()
        {
            if (!LastLevelScenario.IsValid) return LevelScenarioSaveFile.GetDefault();
            return new LevelScenarioSaveFile(LastLevelScenario);
        }
        
        public void SaveCurrentLevelScenario()
        {
            LastLevelScenario = new LevelScenario()
            {
                DifficultyData = Difficulty,
                Scene = GameDatabase.Global.GetSceneDataFromScene(gameObject.scene, out SceneData data) ? data : GameDatabase.Global.Scenes[0],
                LevelElementsCustomInfos = GetCurrentElementInfos(),
                UseCustomInfos = true,
            };
            
            SaveManager<LevelScenarioSaveFile>.Push();
        }

        private Dictionary<string, ILevelElementInfos> GetCurrentElementInfos()
        {
            return LevelElements
                .AsValueEnumerable()
                .ToDictionary(ctx => ctx.GUID, ctx => ctx.Pull());
        }
    }
}