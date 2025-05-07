using LTX.ChanneledProperties;
using RogueLike;
using RogueLike.Controllers;
using RogueLike.Managers;
using UnityEngine;

namespace DeadLink.Menus.New.Implementation
{
    public class PauseMenu : Menu
    {
        public override MenuType MenuType { get; protected set; } = MenuType.Pause;

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
            IMenu menu = MenuManager.Instance.GetMenu(GameMetrics.Global.PauseMenu);
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