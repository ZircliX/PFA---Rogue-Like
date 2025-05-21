using DeadLink.Level.Interfaces;
using DeadLink.Misc;
using DeadLink.Save.LevelProgression;
using Enemy;
using LTX.ChanneledProperties;
using RogueLike.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DeadLink.Level
{
    public class MainMenuLevelScenarioProvider : MonoBehaviour, ILevelScenarioProvider
    {
        public LevelScenario LevelScenario { get; private set; }
        
        public LevelScenario GetLevelScenario(LevelManager levelManager)
        {
            levelManager.LevelScenarioProvider.RemovePriority(this);
            Destroy(gameObject);
            return LevelScenario;
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
        }
        
        private void SceneManagerOnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (LevelManager.HasInstance && LevelScenario.IsValid)
            {
                LevelManager.Instance.LevelScenarioProvider.AddPriority(this, PriorityTags.Highest, this);
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
        
        public void SetLevelScenarioToSavedFile()
        {
            if (LevelScenarioSaveFileListener.CurrentLevelScenarioSaveFile.IsValid)
            {
                LevelScenario = new LevelScenario(LevelScenarioSaveFileListener.CurrentLevelScenarioSaveFile)
                {
                    UseCustomInfos = true,
                };
            }
        }
        
        public void SetDifficultyForNewGame(DifficultyData difficultyData)
        {
            LevelScenario = new LevelScenario()
            {
                DifficultyData = difficultyData,
                Scene = LevelScenario.Scene,
                LevelElementsCustomInfos = LevelScenario.LevelElementsCustomInfos ?? new(),
                UseCustomInfos = false,
            };
        }
        
        public void SetSceneForNewGame(SceneData sceneData)
        {
            Debug.Log("set scene");
            LevelScenario = new LevelScenario()
            {
                DifficultyData = LevelScenario.DifficultyData,
                Scene = sceneData,
                LevelElementsCustomInfos = LevelScenario.LevelElementsCustomInfos ?? new(),
                UseCustomInfos = false,
            };
        }
    }
}