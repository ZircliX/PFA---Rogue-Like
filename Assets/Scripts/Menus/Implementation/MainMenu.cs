using LTX.ChanneledProperties;
using RogueLike;
using RogueLike.Controllers;
using UnityEngine;

namespace DeadLink.Menus.Implementation
{
    public class MainMenu : Menu
    {
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
        }
        
        public void Play()
        {
            SceneController.Global.ChangeScene(GameMetrics.Global.LevelOne);
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
            IMenu menu = MenuManager.Instance.GetMenu(GameMetrics.Global.ScoreboardMenu);
            MenuManager.Instance.OpenMenu(menu);
        }
    }
}