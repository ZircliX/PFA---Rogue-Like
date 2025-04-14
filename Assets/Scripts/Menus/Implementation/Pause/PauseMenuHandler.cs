using RogueLike;
using RogueLike.Controllers;
using UnityEngine;

namespace DeadLink.Menus.Implementation
{
    public class PauseMenuHandler : MenuHandler<PauseMenuContext>
    {
        public override PauseMenuContext GetContext()
        {
            return new PauseMenuContext()
            {
                GameObject = gameObject,
                CursorLockMode = CursorLockMode.None,
                CursorVisibility = true
            };
        }

        public override Menu<PauseMenuContext> GetMenu()
        {
            return new PauseMenu();
        }
        
        public void Resume()
        {
            MenuManager.Instance.CloseMenu();
        }

        public void Settings()
        {
            MenuManager.Instance.ChangeMenu(MenuManager.Instance.Menus["Settings"]);
        }

        public void Quit()
        {
            SceneController.Global.ChangeScene(GameMetrics.Global.MainMenuScene);
        }
    }
}