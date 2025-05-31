using System;
using DeadLink.Level;
using DeadLink.Save.LevelProgression;
using DeadLink.SceneManagement;
using LTX.ChanneledProperties;
using RogueLike;
using SaveSystem.Core;
using UnityEngine;

namespace DeadLink.Menus.Implementation
{
    public class DieMenu : Menu
    {
        public override MenuType MenuType { get; protected set; }
        public override MenuProperties GetMenuProperties()
        {
            return new MenuProperties(
                gameObject,
                PriorityTags.High,
                1f,
                CursorLockMode.None,
                true,
                false,
                false);
        }

        private void Awake()
        {
            MenuType = MenuType.DieMenu;
        }

        public override void Open()
        {
            base.Open();
            LevelScenarioSaveFileListener.CurrentLevelScenarioSaveFile = LevelScenarioSaveFile.GetDefault();
            SaveManager<LevelScenarioSaveFile>.Push();
        }

        public void Restart()
        {
            SceneController.Global.ResetNextSceneIndex();
            SceneController.Global.ChangeScene(GameMetrics.Global.LevelOne);
        }

        public void Menu()
        {
            SceneController.Global.ChangeScene(GameMetrics.Global.MainMenuScene);
            LevelScenarioSaveFileListener.CurrentLevelScenarioSaveFile = LevelScenarioSaveFile.GetDefault();
        }
    }
}