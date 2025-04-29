using LTX.ChanneledProperties;
using RogueLike;
using RogueLike.Controllers;
using RogueLike.Managers;
using UnityEngine;

namespace DeadLink.Menus.Implementation
{
    public class PauseMenuHandler : MenuHandler<PauseMenuContext>
    {
        [field: SerializeField] protected override bool baseState { get; set; }

        public override MenuType MenuType => MenuType.Pause;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void CheckMenuType(MenuType type)
        {
            if (type == GameMetrics.Global.PauseMenu)
            {
                PauseMenu menu = new PauseMenu();
                MenuManager.Instance.OpenMenu(menu, this);
            }
        }

        public override PauseMenuContext GetContext()
        {
            return new PauseMenuContext()
            {
                GameObject = gameObject,
                MenuType = MenuType,
                Priority = PriorityTags.Default,
                CursorLockMode = CursorLockMode.None,
                CursorVisibility = true,
                TimeScale = 0f,
                CanClose = true,
                CanStack = true,
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

        public void Retry()
        {
            LevelManager.Instance.RetryLevel();
        }

        public void Settings()
        {
            MenuManager.Instance.ChangeMenu(GameMetrics.Global.SettingsMenu);
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