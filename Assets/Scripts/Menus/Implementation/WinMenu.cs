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
    public class WinMenu : Menu
    {
        public override MenuType MenuType { get; protected set; }
        public override MenuProperties GetMenuProperties()
        {
            return new MenuProperties(
                gameObject,
                PriorityTags.Default,
                0f,
                CursorLockMode.None,
                true,
                true,
                false);
        }

        private void Awake()
        {
            MenuType = GameMetrics.Global.WinningMenu;
        }

        public void Menu()
        {
            LevelScenarioSaveFileListener.CurrentLevelScenarioSaveFile = LevelScenarioSaveFile.GetDefault();
            SaveManager<LevelScenarioSaveFile>.Push();
            SceneController.Global.ChangeScene(GameMetrics.Global.MainMenuScene);
        }
    }
}