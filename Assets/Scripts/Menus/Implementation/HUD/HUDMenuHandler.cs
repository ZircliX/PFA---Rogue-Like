using RogueLike;
using RogueLike.Controllers;
using UnityEngine;

namespace DeadLink.Menus.Implementation
{
    public class HUDMenuHandler : MenuHandler<HUDMenuContext>
    {
        [field: SerializeField] protected override bool baseState { get; set; }

        public override MenuType MenuType => MenuType.HUD;

        protected override void Awake()
        {
            base.Awake();
            GameController.CursorVisibility.AddPriority(GameMetrics.Global.HUD, this.GetContext().Priority, false);
            GameController.CursorLockMode.AddPriority(GameMetrics.Global.HUD, this.GetContext().Priority,
                CursorLockMode.Locked);
            GameController.TimeScale.AddPriority(GameMetrics.Global.HUD, this.GetContext().Priority, 1f);
        }

        protected override void CheckMenuType(MenuType type)
        {
            if (type == GameMetrics.Global.HUD)
            {
                HUDMenu menu = new HUDMenu();
                MenuManager.Instance.OpenMenu(menu, this);
            }
        }

        public override HUDMenuContext GetContext()
        {
            return new HUDMenuContext
            {
                GameObject = gameObject,
                CursorLockMode = CursorLockMode.Locked,
                CursorVisibility = false
            };
        }

        public override Menu<HUDMenuContext> GetMenu()
        {
            return new HUDMenu();
        }
    }
}