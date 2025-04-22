using DG.Tweening;
using RogueLike;
using UnityEngine;
using UnityEngine.UI;

namespace DeadLink.Menus.Implementation
{
    public class HUDMenuHandler : MenuHandler<HUDMenuContext>
    {
        [field: SerializeField] protected override bool baseState { get; set; }
        
        [Header("Crosshairs")]
        [SerializeField] private CrosshairOffset[] crosshairOffsets;
        
        [Header("Ammunitions")]
        [SerializeField] private AmmunitionReferences[] ammunitions;
        
        [Header("Health")]
        [SerializeField] private Image[] healthBars;
        [SerializeField] private Color activeColor, disableColor;
        [SerializeField] private Image health;
        
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

        public void UpdateHealth(float current, float maxHealth, int healthBarCount)
        {
            for (int i = 0; i < healthBars.Length; i++)
            {
                healthBars[i].gameObject.SetActive(i < healthBarCount);
                healthBars[i].color = i == healthBarCount - 1 ? activeColor : disableColor;
            }

            DOTween.To(
                () => health.fillAmount,
                x => health.fillAmount = x,
                current / maxHealth,
                0.25f).SetEase(Ease.OutSine);
        }
    }
}