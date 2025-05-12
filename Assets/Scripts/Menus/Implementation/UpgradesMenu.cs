using System.Collections.Generic;
using DeadLink.Menus.Other;
using DeadLink.PowerUpSystem;
using DG.Tweening;
using LTX.ChanneledProperties;
using RogueLike.Controllers;
using RogueLike.Managers;
using UnityEngine;
using UnityEngine.Pool;

namespace DeadLink.Menus.Implementation
{
    public class UpgradesMenu : Menu
    {
        [SerializeField] private UpgradePrefab upgradePrefab;
        [SerializeField] private Transform targetUpgradePanel;
        public PowerUp[] PowerUps { get; private set; }
        private List<UpgradePrefab> upgradeUIs;
        
        public override MenuType MenuType { get; protected set; }

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
        
        private void Awake()
        {
            MenuType = MenuType.Upgrades;
            upgradeUIs = new List<UpgradePrefab>();
        }

        public override void Open()
        {
            Debug.Log("Opening Upgrades Menu");
            base.Open();
            SetPowerUps();
        }

        private void GetPowerUps()
        {
            PowerUps = Resources.LoadAll<PowerUp>("PowerUps");
            Debug.Log(PowerUps.Length);
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
                
                if (ui == upgradeInstance)
                {
                    ui.transform.DOMoveY(ui.transform.position.y, 5f).OnComplete(() =>
                    {
                        upgradeUIs.Remove(ui);
                        Destroy(ui.gameObject);
                        SceneController.Global.ChangeScene(SceneController.Global.previousScene.buildIndex);
                    });
                    ui.powerUp.OnBeUnlocked(LevelManager.Instance.Player, LevelManager.Instance.PlayerMovement);
                }
                else
                {
                    ui.transform.DOMoveY(ui.transform.position.y + 1000, 1f).OnComplete(() =>
                    {
                        upgradeUIs.Remove(ui);
                        Destroy(ui.gameObject);
                    });
                }
            }
        }
    }
}