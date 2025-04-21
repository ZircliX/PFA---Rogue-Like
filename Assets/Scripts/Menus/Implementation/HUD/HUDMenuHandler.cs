using RogueLike;
using UnityEngine;

namespace DeadLink.Menus.Implementation
{
    public class HUDMenuHandler : MenuHandler<HUDMenuContext>
    {
        [field: SerializeField] protected override bool baseState { get; set; }
        [SerializeField] private CrosshairOffset[] crosshairOffsets;
        [SerializeField] private AmmunitionReferences[] ammunitions;
        private int currentWeaponIndex;

        public override MenuType MenuType => MenuType.HUD;

        protected override void Awake()
        {
            base.Awake();
            currentWeaponIndex = 0;
        }

        protected override void CheckMenuType(MenuType type)
        {
            if (type == GameMetrics.Global.HUDMenu)
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
                TimeScale = 0f,
                CursorLockMode = CursorLockMode.Locked,
                CursorVisibility = false,
                CanClose = false,
                CanStack = true,
            };
        }

        public override Menu<HUDMenuContext> GetMenu()
        {
            return new HUDMenu();
        }
        
        public void ChangeWeapon(int index)
        {
            crosshairOffsets[currentWeaponIndex].gameObject.SetActive(false);
            crosshairOffsets[index].gameObject.SetActive(true);
            
            ammunitions[currentWeaponIndex].gameObject.SetActive(false);
            ammunitions[index].gameObject.SetActive(true);
            
            currentWeaponIndex = index;
        }
        
        public void UpdateAmmunitions(int current, int max)
        {
            ammunitions[currentWeaponIndex].SetAmmos(current, max);
        }
        
        public void SetCrosshairOffset()
        {
            crosshairOffsets[currentWeaponIndex].FireOffset();
        }
    }
}