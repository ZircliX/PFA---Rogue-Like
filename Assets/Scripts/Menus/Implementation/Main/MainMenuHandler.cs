using RogueLike;
using RogueLike.Controllers;
using UnityEngine;

namespace DeadLink.Menus.Implementation
{
    public class MainMenuHandler : MenuHandler<MainMenuContext>
    {
        public override MainMenuContext GetContext()
        {
            return new MainMenuContext()
            {
                GameObject = gameObject,
                CursorLockMode = CursorLockMode.None,
                CursorVisibility = true
            };
        }

        public override Menu<MainMenuContext> GetMenu()
        {
            return new MainMenu();
        }
        
        public void Play()
        {
            SceneController.Global.ChangeScene(GameMetrics.Global.LevelScene);
        }

        public void Quit()
        {
            GameController.QuitGame();
        }

        public void Settings()
        {
            MenuManager.Instance.ChangeMenu(MenuManager.Instance.Menus["Settings"]);
        }
    }
}