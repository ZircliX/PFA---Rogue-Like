using RogueLike;
using RogueLike.Controllers;
using UnityEngine;

namespace DeadLink.Menus.Implementation
{
    public class UpgradeMenuHandler : MenuHandler<UpgradeMenuContext>
    {
        [field: SerializeField] protected override bool baseState { get; set; }
        [field: SerializeField] public PowerUp.PowerUp[] PowerUps { get; private set; }

        public override MenuType MenuType => MenuType.Upgrades;

        protected override void Awake()
        {
            base.Awake();
            GameController.CursorVisibility.AddPriority(GameMetrics.Global.Upgrades, this.GetContext().Priority, false);
            GameController.CursorLockMode.AddPriority(GameMetrics.Global.Upgrades, this.GetContext().Priority,
                CursorLockMode.Locked);
            GameController.TimeScale.AddPriority(GameMetrics.Global.Upgrades, this.GetContext().Priority, 1f);
        }

        protected override void CheckMenuType(MenuType type)
        {
            if (type == GameMetrics.Global.Upgrades)
            {
                UpgradeMenu menu = new UpgradeMenu();
                MenuManager.Instance.OpenMenu(menu, this);
            }
        }

        public override UpgradeMenuContext GetContext()
        {
            return new UpgradeMenuContext()
            {
                GameObject = gameObject,
                CursorLockMode = CursorLockMode.None,
                CursorVisibility = true
            };
        }

        public override Menu<UpgradeMenuContext> GetMenu()
        {
            return new UpgradeMenu();
        }

        public void UseUpgrade(int index)
        {
            //PowerUps[index].Visit();
        }
    }
}