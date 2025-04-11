using RogueLike;
using RogueLike.Controllers;

namespace DeadLink.Menus.Implementation
{
    public class MainMenu : Menu
    {
        public override bool CanClose { get; protected set; } = false;
        
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
            MenuManager.Instance.OpenMenu(MenuManager.Instance.Menus["Settings"]);
        }
    }
}