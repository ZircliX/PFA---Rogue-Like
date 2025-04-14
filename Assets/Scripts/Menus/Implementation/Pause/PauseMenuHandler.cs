using LTX.ChanneledProperties;
using RogueLike;
using RogueLike.Controllers;
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
            GameController.CursorVisibility.AddPriority(GameMetrics.Global.Pause, this.GetContext().Priority, false);
            GameController.CursorLockMode.AddPriority(GameMetrics.Global.Pause, this.GetContext().Priority,
                CursorLockMode.Locked);
            GameController.TimeScale.AddPriority(GameMetrics.Global.Pause, this.GetContext().Priority, 1f);
        }

        protected override void CheckMenuType(MenuType type)
        {
            if (type == GameMetrics.Global.Pause)
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
            MenuManager.Instance.CloseMenu(MenuType.Pause);
        }

        public void Settings()
        {
            MenuManager.Instance.ChangeMenu(GameMetrics.Global.Settings);
        }

        public void Quit()
        {
            SceneController.Global.ChangeScene(GameMetrics.Global.MainMenuScene);
        }
    }
}