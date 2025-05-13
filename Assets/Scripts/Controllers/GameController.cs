using DeadLink.Save.Settings;
using DG.Tweening;
using LTX.ChanneledProperties;
using RogueLike.Save;
using SaveSystem.Core;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace RogueLike.Controllers
{
    public static class GameController
    {
        public static bool IsPlaying { get; private set; }
        public static void End() => IsPlaying = false;
        public static void Play() => IsPlaying = true;
        
        public static GameProgressionSaveListener GameProgressionSaveListener { get; private set; }
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
            //DOTween.Init(false, false, LogBehaviour.Verbose);

            SetupFields();
            SetupPrioritisedProperties();
            SaveManager<SettingsSave>.SetSaveController(new SaveController());
            SaveManager<SettingsSave>.AddListener(SettingsListener);
            SaveManager<SettingsSave>.Pull();
            
            SaveManager<GameProgression>.SetSaveController(new SaveController());
            SaveManager<GameProgression>.AddListener(GameProgressionSaveListener);
            SaveManager<GameProgression>.Pull();
        }

        public static void QuitGame()
        {
            SaveManager<GameProgression>.Push();
            SaveManager<GameProgression>.RemoveListener(GameProgressionSaveListener);
            SaveManager<SettingsSave>.Push();
            SaveManager<SettingsSave>.RemoveListener(SettingsListener);
            Application.Quit();
        }

        private static void SetupFields()
        {
            GameProgressionSaveListener = new GameProgressionSaveListener();
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