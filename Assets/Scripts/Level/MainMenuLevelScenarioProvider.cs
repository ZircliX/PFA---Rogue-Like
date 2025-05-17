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
                LevelManager.Instance.LevelScenarioProvider.AddPriority(this, PriorityTags.Highest);
                LevelManager.Instance.LevelScenarioProvider.Write(this, this);
            }
        }

        private void Awake()
        {
            SetLevelScenario();
            DontDestroyOnLoad(this);
        }
        
        private void SetLevelScenario()
        {
            if (LevelScenarioSaveFileListener.CurrentLevelScenarioSaveFile.IsValid)
            {
                LevelScenario = new LevelScenario(LevelScenarioSaveFileListener.CurrentLevelScenarioSaveFile);
            }
        }
        
        public void SetDifficulty(DifficultyData difficultyData)
        {
            LevelScenario = new LevelScenario()
            {
                DifficultyData = difficultyData,
                Scene = LevelScenario.Scene,
                LevelElements = LevelScenario.LevelElements
            };
        }
        
        public void SetScene(SceneData sceneData)
        {
            LevelScenario = new LevelScenario()
            {
                DifficultyData = LevelScenario.DifficultyData,
                Scene = sceneData,
                LevelElements = LevelScenario.LevelElements
            };
        }
    }
}