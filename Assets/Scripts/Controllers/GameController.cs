using DeadLink.Level;
using DeadLink.Save.GameProgression;
using DeadLink.Save.LevelProgression;
using DeadLink.Save.Settings;
using DeadLink.SceneManagement;
using LTX.ChanneledProperties;
using SaveSystem.Core;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;

namespace RogueLike.Controllers
{
    [System.Serializable] public enum GameState { MainMenu, Loading, Playing}
    public static class GameController
    {
        public static GameState CurrentState { get; private set; }
        public static void SetGameState(GameState state)
        {
            CurrentState = state;
        }
        
        public static GameDatabase GameDatabase { get; private set; }
        public static GameProgressionListener GameProgressionListener { get; private set; }
        public static LevelScenarioSaveFileListener LevelScenarioSaveFileListener { get; private set; }
        public static SettingsListener SettingsListener { get; private set; }
        public static AudioManager AudioManager { get; private set; }
        public static SceneController SceneController { get; private set; }

        private static GameMetrics gameMetrics;
        public static GameMetrics Metrics
        {
            get
            {
                if (!gameMetrics)
                    gameMetrics = LoadMetrics();

                return gameMetrics;
            }
        }

        private static GameMetrics LoadMetrics() => Resources.Load<GameMetrics>("GameMetrics");
        
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void LoadInEditor()
        {
            gameMetrics = LoadMetrics();
        }
#endif

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void LoadGame()
        {
            Application.quitting += QuitGame;
            SetGameState(GameState.MainMenu);

            SetupFields();
            SetupPrioritisedProperties();
            AddSaveControllers();
            
            GameDatabase.Load();
        }

        public static void QuitGame()
        {
            RemoveSaveControllers();
            
            Application.Quit();
        }

        private static void AddSaveControllers()
        {
            SaveManager<GameProgression>.SetSaveController(new GameSaveController());
            SaveManager<GameProgression>.AddListener(GameProgressionListener);
            SaveManager<GameProgression>.Pull();
            
            SaveManager<SettingsSave>.SetSaveController(new SettingsSaveController());
            SaveManager<SettingsSave>.AddListener(SettingsListener);
            SaveManager<SettingsSave>.Pull();
            
            SaveManager<LevelScenarioSaveFile>.SetSaveController(new LevelScenarioSaveController());
            SaveManager<LevelScenarioSaveFile>.AddListener(LevelScenarioSaveFileListener);
            SaveManager<LevelScenarioSaveFile>.Pull();
        }
        
        private static void RemoveSaveControllers()
        {
            SaveManager<GameProgression>.Push();
            SaveManager<GameProgression>.RemoveListener(GameProgressionListener);
            
            SaveManager<SettingsSave>.Push();
            SaveManager<SettingsSave>.RemoveListener(SettingsListener);

            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                SaveManager<LevelScenarioSaveFile>.Push();
            }
            SaveManager<LevelScenarioSaveFile>.RemoveListener(LevelScenarioSaveFileListener);
        }

        private static void SetupFields()
        {
            GameDatabase = new GameDatabase();
            GameProgressionListener = new GameProgressionListener();
            LevelScenarioSaveFileListener = new LevelScenarioSaveFileListener();
            SettingsListener = new SettingsListener();
            
            AudioManager = new AudioManager();
            SceneController = new SceneController();
        }
        
        #region Prioritised Properties

        public static PrioritisedProperty<float> TimeScale { get; private set; }
        public static PrioritisedProperty<CursorLockMode> CursorLockMode { get; private set; }
        public static PrioritisedProperty<bool> CursorVisibility { get; private set; }

        private static void SetupPrioritisedProperties()
        {
            TimeScale = new PrioritisedProperty<float>(1f);
            TimeScale.AddOnValueChangeCallback(UpdateTimeScale, true);

            CursorLockMode = new PrioritisedProperty<CursorLockMode>(UnityEngine.CursorLockMode.Locked);
            CursorLockMode.AddOnValueChangeCallback(UpdateCursorLockMode, true);
            
            CursorVisibility = new PrioritisedProperty<bool>(false);
            CursorVisibility.AddOnValueChangeCallback(UpdateCursorVisibility, true);
        }

        private static void UpdateTimeScale(float value)
        {
            Time.timeScale = value;
        }
        
        private static void UpdateCursorLockMode(CursorLockMode value)
        {
            Cursor.lockState = value;
        }
        
        private static void UpdateCursorVisibility(bool value)
        {
            Cursor.visible = value;
        }
        
        #endregion
    }
}