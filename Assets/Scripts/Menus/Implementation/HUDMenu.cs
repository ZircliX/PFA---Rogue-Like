using DeadLink.Menus.Other;
using DeadLink.PowerUpSystem;
using DeadLink.Save.LevelProgression;
using DG.Tweening;
using LTX.ChanneledProperties;
using RogueLike.Controllers;
using RogueLike.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CrosshairOffset = DeadLink.Menus.Other.CrosshairOffset;

namespace DeadLink.Menus.Implementation
{
    public class HUDMenu : Menu
    {
        [Header("Crosshairs")]
        [SerializeField] private CrosshairOffset[] crosshairOffsets;
        
        [Header("Ammunitions")]
        [SerializeField] private AmmunitionReferences[] ammunitions;
        
        [Header("Ammunitions")]
        [SerializeField] private WeaponReference[] weapons;
        
        [Header("Health")]
        [SerializeField] private Image[] healthBars;
        [SerializeField] private Color activeColor, disableColor;
        [SerializeField] private Image health;
        [SerializeField] private CanvasGroup warning;
        
        [Header("Power ups")]
        [SerializeField] private UpgradeBadgeSlot[] upgradeBadgeSlots;
        [SerializeField] private UpgradeBadge upgradeBadgePrefab;
        
        private int currentWeaponIndex;

        public override MenuType MenuType { get; protected set; }

        private void Awake()
        {
            MenuType = MenuType.HUD;
        }

        public override void Open()
        {
            base.Open();
            GameController.SetGameState(GameState.Playing);
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
            
            weapons[currentWeaponIndex].DeactivateImage();
            weapons[index].ActivateImage();
            
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

        private Sequence warningSequence;
        public void HandleWarning()
        {
            //if (warningSequence != null || warningSequence.IsPlaying()) return;
            
            warningSequence = DOTween.Sequence();

            for (int i = 0; i < 3; i++)
            {
                warningSequence.Append(warning.DOFade(1f, 0.15f)).Append(warning.DOFade(0f, 0.15f));
            }

            warningSequence.Play().OnComplete(() =>
            {
                warning.alpha = 0f;
                warningSequence = null;
            });
        }

        public void AddBadges()
        {
            for (int i = 0; i < upgradeBadgeSlots.Length; i++)
            {
                upgradeBadgeSlots[i].Delete();
            }
            
            //Debug.Log("Show Badges");
            Debug.Log(LevelManager.Instance.PlayerController.PlayerEntity.PowerUps.Count);
            
            foreach (PowerUp pu in LevelManager.Instance.PlayerController.PlayerEntity.PowerUps)
            {
                if (TryGetNextPowerUpSlot(out UpgradeBadgeSlot slot))
                {
                    //Debug.Log($"Slot {slot.name}, pu : {pu.Name}");
                    slot.SetUpgradeBadge(upgradeBadgePrefab, pu);
                }
            }
        }

        public bool TryGetNextPowerUpSlot(out UpgradeBadgeSlot upgradeBadge)
        {
            for (int i = 0; i < upgradeBadgeSlots.Length; i++)
            {
                if (upgradeBadgeSlots[i].IsEmpty)
                {
                    upgradeBadge = upgradeBadgeSlots[i];
                    return true;
                }
            }

            upgradeBadge = null;
            return false;
        }
    }
}