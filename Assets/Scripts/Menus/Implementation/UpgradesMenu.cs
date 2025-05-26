using System.Collections.Generic;
using DeadLink.Menus.Other;
using DeadLink.PowerUpSystem;
using DG.Tweening;
using LTX.ChanneledProperties;
using RogueLike.Managers;
using UnityEngine;
using UnityEngine.Pool;

namespace DeadLink.Menus.Implementation
{
    public class UpgradesMenu : Menu
    {
        [SerializeField] private UpgradePrefab upgradePrefab;
        [SerializeField] private UpgradeBadge upgradeBadge;
        [SerializeField] private Transform targetUpgradePanel;
        [SerializeField] private RectTransform middle;
        public PowerUp[] PowerUps { get; private set; }
        private List<UpgradePrefab> upgradeUIs;
        
        public override MenuType MenuType { get; protected set; }
        private bool canBeClosed = false;

        public override MenuProperties GetMenuProperties()
        {
            return new MenuProperties(
                gameObject,
                PriorityTags.Default,
                0f,
                CursorLockMode.None,
                true,
                canBeClosed,
                false);
        }
        
        private void Awake()
        {
            MenuType = MenuType.Upgrades;
            upgradeUIs = new List<UpgradePrefab>();
        }

        public override void Open()
        {
            base.Open();
            SetPowerUps();
        }

        private void GetPowerUps()
        {
            PowerUps = Resources.LoadAll<PowerUp>("PowerUps");
        }
        
        public void SetPowerUps()
        {
            GetPowerUps();
            using (ListPool<PowerUp>.Get(out List<PowerUp> pow))
            {
                pow.AddRange(PowerUps);
                int count = pow.Count >= 3 ? 3 : pow.Count;
                
                for (int i = 0; i < count; i++)
                {
                    int index = Random.Range(0, pow.Count);
                    PowerUp powerUp = pow[index];
                    pow.RemoveAt(index);
                    
                    UpgradePrefab upgradeUI = Instantiate(upgradePrefab, targetUpgradePanel);
                    //Debug.Log("powerUp : " + powerUp.Name);
                    upgradeUI.Initialize(this, powerUp);

                    //Debug.Log(upgradeUI);
                    upgradeUIs.Add(upgradeUI);
                }
            }
        }
        
        public void UseUpgrade(UpgradePrefab upgradeInstance)
        {
            for (int i = upgradeUIs.Count - 1; i >= 0; i--)
            {
                UpgradePrefab ui = upgradeUIs[i];
                Debug.Log(ui.layout);
                ui.layout.ignoreLayout = true;
                
                if (ui == upgradeInstance)
                {
                    ui.transform.DOMove(middle.position, 1.5f).SetTarget(ui.transform).SetUpdate(true).OnComplete(() =>
                    {
                        AddBadge(ui);
                    });
                }
                else
                {
                    ui.transform.DOMoveY(ui.transform.position.y + 3000, 1.5f).SetTarget(ui.transform).SetUpdate(true).OnComplete(() =>
                    {
                        upgradeUIs.Remove(ui);
                        Destroy(ui.gameObject);
                    });
                }
            }
        }

        private void AddBadge(UpgradePrefab upgrade)
        {
            upgrade.powerUp.OnBeUnlocked(LevelManager.Instance.PlayerController.PlayerEntity, LevelManager.Instance.PlayerController.PlayerMovement);
            
            UpgradeBadge badge = Instantiate(upgradeBadge, middle);
            badge.transform.SetSiblingIndex(0);
            badge.SetImage(upgrade.powerUp.Badge);
            
            if (upgrade.transform == null) return;
            upgrade.transform.DOScale(Vector3.zero, 0.5f).SetUpdate(true).OnComplete(() =>
            {
                bool tryGetNextPowerUpPosition = MenuManager.Instance.HUDMenu.TryGetNextPowerUpPosition(out Transform target);
                if (tryGetNextPowerUpPosition)
                {
                    badge.transform.DOMove(target.position, 0.75f).SetDelay(0.25f).SetUpdate(true).OnComplete(() =>
                    {
                        DeleteUpgrade(upgrade);
                    });
                }
            });
        }

        private void DeleteUpgrade(UpgradePrefab upgrade)
        {
            upgrade.DOKill();
            upgradeUIs.Remove(upgrade);
            Destroy(upgrade.gameObject);
            
            canBeClosed = true;
            MenuManager.Instance.CloseMenu();
        }
    }
}