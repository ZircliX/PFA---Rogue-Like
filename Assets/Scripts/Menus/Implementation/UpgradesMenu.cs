using System.Collections.Generic;
using DeadLink.Menus.Implementation;
using DeadLink.PowerUpSystem;
using DG.Tweening;
using LTX.ChanneledProperties;
using UnityEngine;
using UnityEngine.Pool;

namespace DeadLink.Menus.New.Implementation
{
    public class UpgradesMenu : Menu
    {
        [SerializeField] private UpgradePrefab upgradePrefab;
        [SerializeField] private Transform targetUpgradePanel;
        public PowerUp[] PowerUps { get; private set; }
        private List<UpgradePrefab> upgradeUIs;
        
        public override MenuType MenuType { get; protected set; } = MenuType.Upgrades;

        public override MenuProperties GetMenuProperties()
        {
            return new MenuProperties(
                gameObject,
                PriorityTags.Default,
                0f,
                CursorLockMode.None,
                true,
                false,
                false);
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
                
                float moveValue = i != index ? 50 : -50;
                ui.transform.DOMoveY(moveValue, 0.5f).OnComplete(() =>
                {
                    Destroy(ui.gameObject);
                    upgradeUIs.RemoveAt(i);
                });
            }
        }
    }
}