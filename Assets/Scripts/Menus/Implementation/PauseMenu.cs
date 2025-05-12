using System;
using LTX.ChanneledProperties;
using RogueLike;
using RogueLike.Controllers;
using RogueLike.Managers;
using UnityEngine;

namespace DeadLink.Menus.New.Implementation
{
    public class PauseMenu : Menu
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
                true);
        }

        private void Awake()
        {
            MenuType = MenuType.Pause;
        }

        public void Resume()
        {
            MenuManager.Instance.CloseMenu();
        }

        public void Retry()
        {
            LevelManager.Instance.RetryLevel();
        }

        public void Settings()
        {
            IMenu menu = MenuManager.Instance.GetMenu(GameMetrics.Global.SettingsMenu);
            //Debug.Log($"menu : {menu}, menuType : {menu.MenuType}, menuName : {menu.GetMenuProperties().GameObject}");
            MenuManager.Instance.OpenMenu(menu);
        }

        public void MainMenu()
        {
            SceneController.Global.ChangeScene(GameMetrics.Global.MainMenuScene);
        }

        public void Quit()
        {
            GameController.QuitGame();
        }
    }
}