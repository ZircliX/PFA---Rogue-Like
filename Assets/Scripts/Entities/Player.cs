using System.Collections.Generic;
using DeadLink.Entities;
using DeadLink.Entities.Data;
using DeadLink.Menus;
using DeadLink.Menus.New;
using DeadLink.PowerUpSystem;
using DeadLink.PowerUpSystem.InterfacePowerUps;
using Enemy;
using KBCore.Refs;
using LTX.ChanneledProperties;
using RogueLike.Managers;
using RogueLike.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RogueLike.Entities
{
    public class Player : Entity
    {
        [field : SerializeField] public Transform SpawnPosition { get; private set; }
        [field : SerializeField] public List<string> PowerUpsInputName{ get; private set; }
        private Dictionary<string, IVisitor> unlockedPowerUps;
        private Dictionary<string, string> inputToPowerUpName;

        [SerializeField, Self] private PlayerMovement pm;

        public int Kills { get; private set; }

        #region Event Functions
        
        private void OnValidate() => this.ValidateRefs();
        
        private void Start()
        {
            if (PowerUpsInputName.Count < GameMetrics.Global.PowerUps.Length) return;

            for (int i = 0; i < GameMetrics.Global.PowerUps.Length; i++)
            {
                PowerUp powerUp = GameMetrics.Global.PowerUps[i];
                string inputName = PowerUpsInputName[i];

                inputToPowerUpName.Add(inputName, powerUp.Name);
            }
        }

        #endregion
        
        #region spawn damage heal die
        
        public override void Spawn(EntityData data, DifficultyData difficultyData, Vector3 spawnPoint)
        {
            base.Spawn(data, difficultyData, spawnPoint);
            
            HealthBarCount.AddInfluence(difficultyData, difficultyData.PlayerHealthBarCount, Influence.Add);
            MaxHealth.AddInfluence(difficultyData, difficultyData.PlayerHealthMultiplier, Influence.Multiply);
            Strength.AddInfluence(difficultyData, difficultyData.PlayerStrengthMultiplier, Influence.Multiply);
            Resistance.AddInfluence(difficultyData, difficultyData.PlayerResistanceMultiplier, Influence.Multiply);
            Speed.AddInfluence(this, 1, Influence.Add);
            
            unlockedPowerUps = new Dictionary<string, IVisitor>();
            inputToPowerUpName = new Dictionary<string, string>();
            
            ResetPowerUps();
        }

        private void ResetPowerUps()
        {
            foreach (KeyValuePair<string, IVisitor> power in unlockedPowerUps)
            {
                power.Value.OnReset(this, pm);
            }
        }

        protected override void Attack()
        {
            Shoot();
        }

        public override void TakeDamage(int damage)
        {
            int finalDamage = Mathf.CeilToInt(damage / Resistance);
            base.TakeDamage(finalDamage);
            MenuManager.Instance.HUDMenu.UpdateHealth(Health, MaxHealth.Value, HealthBarCount.Value);
        }

        public override void Die()
        {

        }
        
        #endregion
        
        protected override void ChangeWeapon(int direction)
        {
            if (!MenuManager.Instance.TryGetCurrentMenu(out IMenu menu) || menu.MenuType != MenuType.HUD) return;
            
            base.ChangeWeapon(direction);
            
            MenuManager.Instance.HUDMenu.ChangeWeapon(currentWeaponIndex);
            MenuManager.Instance.HUDMenu.UpdateAmmunitions(CurrentWeapon.CurrentMunitions, CurrentWeapon.WeaponData.MaxAmmunition);
        }

        protected override void Reload()
        {
            if (!MenuManager.Instance.TryGetCurrentMenu(out IMenu menu) || menu.MenuType != MenuType.HUD) return;
            base.Reload();
        }

        #region Inputs
        
        public void ChangeWeapon(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                ChangeWeapon((int)context.ReadValue<float>());
            }
        }
        
        public void Shoot(InputAction.CallbackContext context)
        {
            if (context.performed && CurrentWeapon != null)
            {
                isShooting = true;
            }

            if (context.canceled)
            {
                isShooting = false;
            }
        }
        
        public void Reload(InputAction.CallbackContext context)
        {
            if (context.performed && CurrentWeapon != null)
            {
                Reload();
            }
        }
        
        #endregion
        
        #region PowerUps
        
        public override void UsePowerUp(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            string actionName = context.action.name;
            
            if (inputToPowerUpName.TryGetValue(actionName, out string powerUpName))
            {
                Debug.Log($"Touched {powerUpName} found powerUp named {powerUpName}");
                
                if (unlockedPowerUps.TryGetValue(powerUpName, out IVisitor powerUp))
                {
                    Debug.Log($"Entry {powerUpName} found powerUp named {powerUp}");

                    powerUp.OnBeUsed(this, pm);
                }
            }
        }

        public override void OnFixedUpdate()
        {
        }

        public override void Unlock(IVisitor visitor)
        {
            if (!unlockedPowerUps.TryAdd(visitor.Name, visitor))
            {
                Debug.Log("already unlocked");
                return;
            }
            unlockedPowerUps.Add(visitor.Name, visitor);
            visitor.OnBeUnlocked(this, pm);
        }
        
        public void StartCooldownCoroutine(CooldownPowerUp cooldownPowerUp)
        {
            StartCoroutine(cooldownPowerUp.Cooldown());
        }
        
        public void ActiveContinuousFire(CooldownPowerUp cooldownPowerUp)
        {
            if (CurrentWeapon != null)
            {
                ContinuousFire = true; 
            }
        }

        public void DesactiveContinuousFire()
        {
            ContinuousFire = false;
        }
        
        public void ActiveInvisibility()
        {
            IsInvisible = true;
        }
        
        public void DesactiveInvisibility()
        {
            IsInvisible = false;
        }

        public void OnAdrenalineShot(int adrenalineMultipier)
        {
            Speed.Write(this, adrenalineMultipier);
        }
        public void OnAdrenalineShotEnd()
        {
            Speed.Write(this, 1);
        }
        
        #endregion


    }
}