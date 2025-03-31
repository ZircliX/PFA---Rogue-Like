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
        
        public static SaveListener SaveListener { get; private set; }

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

            SetupFields();
            
            SaveManager<SampleSaveFile>.SetSaveController(new SaveController());
            SaveManager<SampleSaveFile>.AddListener(SaveListener);
            SaveManager<SampleSaveFile>.Pull();
        }

        public static void QuitGame()
        {
            SaveManager<SampleSaveFile>.Push();
            SaveManager<SampleSaveFile>.RemoveListener(SaveListener);
            Application.Quit();
        }

        private static void SetupFields()
        {
            SaveListener = new SaveListener();
        }
    }
}