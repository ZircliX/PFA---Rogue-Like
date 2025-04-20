using RogueLike;
using UnityEngine;

namespace DeadLink.Menus.Implementation
{
    public class HUDMenuHandler : MenuHandler<HUDMenuContext>
    {
        [field: SerializeField] protected override bool baseState { get; set; }
        [SerializeField] private GameObject[] crossHairs;
        private int currentCrossHairIndex;

        public override MenuType MenuType => MenuType.HUD;

        protected override void Awake()
        {
            base.Awake();
            currentCrossHairIndex = 0;
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
        
        public void SetCrossHair(int index)
        {
            crossHairs[currentCrossHairIndex].SetActive(false);
            crossHairs[index].SetActive(true);
            currentCrossHairIndex = index;
        }
    }
}