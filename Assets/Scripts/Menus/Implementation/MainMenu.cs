using DeadLink.Level;
using DeadLink.Menus.Other.Scoreboard;
using DeadLink.Save.GameProgression;
using DeadLink.Save.LevelProgression;
using DeadLink.SceneManagement;
using DG.Tweening;
using Enemy;
using LTX.ChanneledProperties;
using RogueLike;
using RogueLike.Controllers;
using SaveSystem.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DeadLink.Menus.Implementation
{
    public class MainMenu : Menu
    {
        [Header("Difficulty")]
        [SerializeField] private CanvasGroup difficultyPanel;
        
        [Header("Name")]
        [SerializeField] private CanvasGroup namePanel;
        [SerializeField] private TMP_InputField nameInputField;
        private string playerName;
        
        [Header("Buttons")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button continueButton;

        [Header("References")]
        private MainMenuLevelScenarioProvider mainMenuLevelScenarioProvider =>
            GameObject.FindAnyObjectByType<MainMenuLevelScenarioProvider>();
        
        public override MenuType MenuType { get; protected set; }

        public override MenuProperties GetMenuProperties()
        {
            return new MenuProperties(
                gameObject,
                PriorityTags.Default,
                1f,
                CursorLockMode.None,
                true,
                false,
                true);
        }
        
        private void Awake()
        {
            MenuType = MenuType.Main;
            ScoreboardSession.Instance.StartSession();
        }

        private void Start()
        {
            //Debug.Log(LevelScenarioSaveFileListener.CurrentLevelScenarioSaveFile.IsValid);
            continueButton.interactable = LevelScenarioSaveFileListener.CurrentLevelScenarioSaveFile.IsValid;
        }
        
        public void UpdatePlayerName()
        {
            playerName = nameInputField.text;
            playButton.interactable = !string.IsNullOrEmpty(playerName);
        }

        public void SetPlayerName()
        {
            GameController.GameProgressionListener.SetPlayerName(playerName);
            ScoreboardSession.Instance.SetPlayerName();
            SaveManager<GameProgression>.Pull();
        }
        
        public void OpenNamePanel()
        {
            OpenCanvasGroup(namePanel);
        }

        public void OpenDifficulty()
        {
            OpenCanvasGroup(difficultyPanel);
        }
        
        private void OpenCanvasGroup(CanvasGroup canvasGroup)
        {
            canvasGroup.interactable = Mathf.Approximately(canvasGroup.alpha, 0);
            canvasGroup.DOFade(Mathf.Approximately(canvasGroup.alpha, 1) ? 0 : 1, 0.25f).SetUpdate(true);
        }

        public void SetDifficulty(int index)
        {
            DifficultyData difficulty = GameController.GameDatabase.Difficulties[index];
            mainMenuLevelScenarioProvider.SetDifficultyForNewGame(difficulty);
        }
        
        public void StartNewGame()
        {
            mainMenuLevelScenarioProvider.SetSceneForNewGame(GameDatabase.Global.GetSceneFromSceneReference(GameMetrics.Global.LevelOne));
            SceneController.Global.ChangeScene(mainMenuLevelScenarioProvider.LevelScenario.Scene);
        }
        
        public void ContinueGame()
        {
            mainMenuLevelScenarioProvider.SetLevelScenarioToSavedFile(); 
            SceneController.Global.ChangeScene(mainMenuLevelScenarioProvider.LevelScenario.Scene.Scene);
        }

        public void Quit()
        {
            GameController.QuitGame();
        }

        public void Settings()
        {
            IMenu menu = MenuManager.Instance.GetMenu(GameMetrics.Global.SettingsMenu);
            MenuManager.Instance.OpenMenu(menu);
        }
        
        public void Credits()
        {
            IMenu menu = MenuManager.Instance.GetMenu(GameMetrics.Global.CreditsMenu);
            MenuManager.Instance.OpenMenu(menu);
        }

        public void Scoreboard()
        {
            IMenu menu = MenuManager.Instance.GetMenu(GameMetrics.Global.GameplayScoreboard);
            MenuManager.Instance.OpenMenu(menu);
        }
    }
}