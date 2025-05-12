using DeadLink.Menus.Implementation;
using DG.Tweening;
using LTX.ChanneledProperties;
using UnityEngine;
using UnityEngine.UI;

namespace DeadLink.Menus.New.Implementation
{
    public class HUDMenu : Menu
    {
        [Header("Crosshairs")]
        [SerializeField] private CrosshairOffset[] crosshairOffsets;
        
        [Header("Ammunitions")]
        [SerializeField] private AmmunitionReferences[] ammunitions;
        
        [Header("Health")]
        [SerializeField] private Image[] healthBars;
        [SerializeField] private Color activeColor, disableColor;
        [SerializeField] private Image health;
        
        private int currentWeaponIndex;

        public override MenuType MenuType { get; protected set; }

        private void Awake()
        {
            MenuType = MenuType.HUD;
            currentWeaponIndex = 0;
        }

        public override MenuProperties GetMenuProperties()
        {
            return new MenuProperties(
                gameObject,
                PriorityTags.Default,
                1f,
                CursorLockMode.Locked,
                false,
                false,
                true);
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