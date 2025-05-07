using LTX.ChanneledProperties;
using RogueLike;
using RogueLike.Controllers;
using UnityEngine;

namespace DeadLink.Menus.New.Implementation
{
    public class MainMenu : Menu
    {
        public override MenuType MenuType { get; protected set; } = MenuType.Main;

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
    }
}