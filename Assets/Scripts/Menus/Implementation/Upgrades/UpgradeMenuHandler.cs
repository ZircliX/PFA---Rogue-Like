using System.Collections.Generic;
using DG.Tweening;
using RogueLike;
using RogueLike.Controllers;
using UnityEngine;

namespace DeadLink.Menus.Implementation
{
    public class UpgradeMenuHandler : MenuHandler<UpgradeMenuContext>
    {
        [field: SerializeField] protected override bool baseState { get; set; }
        [SerializeField] private UpgradePrefab upgradePrefab;
        [SerializeField] private Transform targetUpgradePanel;
        public PowerUp.PowerUp[] PowerUps { get; private set; }
        private List<PowerUp.PowerUp> upgrades;
        private List<UpgradePrefab> upgradeUIs;
        
        public override MenuType MenuType => MenuType.Upgrades;

        protected override void Awake()
        {
            base.Awake();
            GameController.CursorVisibility.AddPriority(GameMetrics.Global.Upgrades, this.GetContext().Priority, false);
            GameController.CursorLockMode.AddPriority(GameMetrics.Global.Upgrades, this.GetContext().Priority,
                CursorLockMode.Locked);
            GameController.TimeScale.AddPriority(GameMetrics.Global.Upgrades, this.GetContext().Priority, 1f);
            
            GetPowerUps();
            SetPowerUps();
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

        private void GetPowerUps()
        {
            PowerUps = Resources.LoadAll<PowerUp.PowerUp>("PowerUps");
        }
        
        public void SetPowerUps()
        {
            upgrades.Clear();
            
            while (upgrades.Count < 3)
            {
                int index = Random.Range(0, PowerUps.Length);
                PowerUp.PowerUp powerUp = PowerUps[index];
                
                if (!upgrades.Contains(powerUp))
                {
                    UpgradePrefab upgradeUI = Instantiate(upgradePrefab, targetUpgradePanel);
                    upgradeUI.Initialize(powerUp.Name, powerUp.Name, null);

                    upgradeUIs.Add(upgradeUI);
                    upgrades.Add(powerUp);
                }
            }
        }
        
        public void UseUpgrade(int index)
        {
            //TODO: Get Visitable Component and pass the power up
            //var comp;
            //upgrades[index].OnBeUnlocked(comp);

            for (int i = upgradeUIs.Count - 1; i >= 0; i--)
            {
                UpgradePrefab ui = upgradeUIs[i];

                if (i != index)
                {
                    ui.transform.DOMoveY(50, 0.5f).OnComplete(() =>
                    {
                        Destroy(ui.gameObject);
                        upgradeUIs.RemoveAt(i);
                        upgrades.RemoveAt(i);
                    });
                }
                else
                {
                    ui.transform.DOMoveY(-50, 0.5f).OnComplete(() =>
                    {
                        Destroy(ui.gameObject);
                        upgradeUIs.RemoveAt(i);
                        upgrades.RemoveAt(i);
                    });
                }
            }
        }
    }
}