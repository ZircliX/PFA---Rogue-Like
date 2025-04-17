using RogueLike;
using RogueLike.Controllers;
using UnityEngine;

namespace DeadLink.Menus.Implementation
{
    public class SettingsMenuHandler : MenuHandler<SettingsMenuContext>
    {
        [field: SerializeField] protected override bool baseState { get; set; }

        public override MenuType MenuType => MenuType.Settings;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void CheckMenuType(MenuType type)
        {
            if (type == GameMetrics.Global.Settings)
            {
                SettingsMenu menu = new SettingsMenu();
                MenuManager.Instance.OpenMenu(menu, this);
            }
        }

        public override SettingsMenuContext GetContext()
        {
            return new SettingsMenuContext
            {
                GameObject = gameObject,
                CursorLockMode = CursorLockMode.None,
                CursorVisibility = true
            };
        }

        public override Menu<SettingsMenuContext> GetMenu()
        {
            return new SettingsMenu();
        }

        public void Back()
        {
            MenuManager.Instance.CloseMenu();
        }
    }
}