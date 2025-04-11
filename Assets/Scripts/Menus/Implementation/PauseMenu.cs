using RogueLike;
using RogueLike.Controllers;

namespace DeadLink.Menus.Implementation
{
    public class PauseMenu : Menu
    {
        public override bool CanClose { get; protected set; } = true;

        public void Resume()
        {
            MenuManager.Instance.CloseMenu();
        }

        public void Settings()
        {
            MenuManager.Instance.OpenMenu(MenuManager.Instance.Menus["Settings"]);
        }

        public void Quit()
        {
            SceneController.Global.ChangeScene(GameMetrics.Global.MainMenuScene);
        }
    }
}