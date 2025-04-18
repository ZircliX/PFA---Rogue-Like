using RogueLike;
using RogueLike.Controllers;
using UnityEngine;

namespace DeadLink.Menus.Implementation
{
    public class MainMenuHandler : MenuHandler<MainMenuContext>
    {
        [field: SerializeField] protected override bool baseState { get; set; }

        public override MenuType MenuType => MenuType.Main;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void CheckMenuType(MenuType type)
        {
            if (type == GameMetrics.Global.MainMenu)
            {
                MainMenu menu = new MainMenu();
                MenuManager.Instance.OpenMenu(menu, this);
            }
        }

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
            SceneController.Global.ChangeScene(GameMetrics.Global.LevelOne);
        }

        public void Quit()
        {
            GameController.QuitGame();
        }

        public void Settings()
        {
            MenuManager.Instance.ChangeMenu(GameMetrics.Global.SettingsMenu);
        }
    }
}