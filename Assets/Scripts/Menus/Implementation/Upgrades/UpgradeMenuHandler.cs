using System.Collections.Generic;
using DeadLink.PowerUpSystem;
using DG.Tweening;
using LTX.ChanneledProperties;
using RogueLike;
using UnityEngine;
using UnityEngine.Pool;

namespace DeadLink.Menus.Implementation
{
    public class UpgradeMenuHandler : MenuHandler<UpgradeMenuContext>
    {
        [field: SerializeField] protected override bool baseState { get; set; }
        [SerializeField] private UpgradePrefab upgradePrefab;
        [SerializeField] private Transform targetUpgradePanel;
        public PowerUp[] PowerUps { get; private set; }
        private List<UpgradePrefab> upgradeUIs;
        
        public override MenuType MenuType => MenuType.Upgrades;

        protected override void Awake()
        {
            base.Awake();
            upgradeUIs = new List<UpgradePrefab>();
        }
        
        protected override void CheckMenuType(MenuType type)
        {
            if (type == GameMetrics.Global.UpgradesMenu)
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
                CursorVisibility = true,
                TimeScale = 0f,
                Priority = PriorityTags.VeryHigh,
                CanClose = false,
                CanStack = false,
                
                handler = this,
            };
        }

        public override Menu<UpgradeMenuContext> GetMenu()
        {
            return new UpgradeMenu();
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
                    upgradeUI.Initialize(powerUp.Name, powerUp.Name, null, powerUp);

                    upgradeUIs.Add(upgradeUI);
                }
            }
        }
        
        public void UseUpgrade(int index)
        {
            for (int i = upgradeUIs.Count - 1; i >= 0; i--)
            {
                UpgradePrefab ui = upgradeUIs[i];
                

                if (i != index)
                {
                    ui.transform.DOMoveY(50, 0.5f).OnComplete(() =>
                    {
                        Destroy(ui.gameObject);
                        upgradeUIs.RemoveAt(i);
                    });
                }
                else
                {
                    ui.transform.DOMoveY(-50, 0.5f).OnComplete(() =>
                    {
                        //ui.powerUp.OnBeUnlocked(ui.powerUp.GetComponent());
                        
                        Destroy(ui.gameObject);
                        upgradeUIs.RemoveAt(i);
                    });
                }
            }
        }

        public void OpenMenu()
        {
            MenuManager.Instance.ChangeMenu(MenuType.Upgrades);
        }
    }
}