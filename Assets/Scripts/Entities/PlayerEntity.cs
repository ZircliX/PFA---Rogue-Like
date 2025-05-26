using System;
using System.Collections;
using System.Collections.Generic;
using DeadLink.Entities;
using DeadLink.Entities.Data;
using DeadLink.Menus;
using DeadLink.PowerUpSystem;
using DeadLink.PowerUpSystem.InterfacePowerUps;
using Enemy;
using KBCore.Refs;
using LTX.ChanneledProperties;
using RogueLike.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RogueLike.Entities
{
    public class PlayerEntity : Entity
    {
        private Dictionary<string, IVisitor> unlockedPowerUps;
        public bool isLastChanceActivated;
        public static Action<PlayerEntity, PlayerMovement> OnPlayerLastChanceUsed;
        
        [SerializeField, Self] private PlayerMovement pm;

        public int Kills { get; private set; }

        #region Event Functions
        
        private void OnValidate() => this.ValidateRefs();

        private void Awake()
        {
            PowerUps = new List<PowerUp>();
        }

        private void Start()
        {
            /*
            if (PowerUpsInputName.Count < GameMetrics.Global.PowerUps.Length) return;

            for (int i = 0; i < GameMetrics.Global.PowerUps.Length; i++)
            {
                PowerUp powerUp = GameMetrics.Global.PowerUps[i];
                string inputName = PowerUpsInputName[i];

                inputToPowerUpName.Add(inputName, powerUp.Name);
            }
            */
        }

        #endregion
        
        #region spawn damage heal die
        
        public override void Spawn(EntityData data, DifficultyData difficultyData, Vector3 spawnPoint)
        {
            base.Spawn(data, difficultyData, spawnPoint);
            MenuManager.Instance.HUDMenu.ChangeWeapon(currentWeaponIndex);
            
            MaxHealthBarCount.AddInfluence(difficultyData, difficultyData.PlayerHealthBarCount, Influence.Multiply);
            MaxHealth.AddInfluence(difficultyData, difficultyData.PlayerHealthMultiplier, Influence.Multiply);
            Strength.AddInfluence(difficultyData, difficultyData.PlayerStrengthMultiplier, Influence.Multiply);
            Resistance.AddInfluence(difficultyData, difficultyData.PlayerResistanceMultiplier, Influence.Multiply);
            Speed.AddInfluence(difficultyData, 1, Influence.Multiply);

            if (!firstSpawn)
            {
                firstSpawn = true;
                Health = MaxHealth.Value;
                HealthBarCount = MaxHealthBarCount.Value;
                MenuManager.Instance.HUDMenu.UpdateHealth(Health, MaxHealth.Value, HealthBarCount);
            }
            
            unlockedPowerUps = new Dictionary<string, IVisitor>();
            
            ResetPowerUps();
        }

        private void ResetPowerUps()
        {
            foreach (KeyValuePair<string, IVisitor> power in unlockedPowerUps)
            {
                power.Value.OnReset(this, pm);
            }
        }
        
        public override bool TakeDamage(int damage, bool byPass = false)
        {
            int finalDamage = byPass ? damage : Mathf.CeilToInt(damage / Resistance);

            if (Health <= MaxHealth.Value * 0.5f && HealthBarCount == 1)
            {
                MenuManager.Instance.HUDMenu.HandleWarning();
            }
            
            bool isDying = Health - finalDamage <= 0;
            bool isLastHealthBar = HealthBarCount <= 1;
            
            if (isDying && isLastHealthBar && isLastChanceActivated)
            {
                OnPlayerLastChanceUsed?.Invoke(this, pm);
                SetHealth(1f);
                MenuManager.Instance.HUDMenu.UpdateHealth(Health, MaxHealth.Value, HealthBarCount);
                return false;
            }
            
            bool die = base.TakeDamage(finalDamage, byPass);
            
            MenuManager.Instance.HUDMenu.UpdateHealth(Health, MaxHealth.Value, HealthBarCount);
            return die;
        }

        protected override void SetHealth(float health)
        {
            base.SetHealth(health);
            MenuManager.Instance.HUDMenu.UpdateHealth(health, MaxHealth.Value, HealthBarCount);
        }

        public bool EmptyHealthBar()
        {
            if (HealthBarCount - 1 <= 0)
            {
                HealthBarCount = 0;
                SetHealth(0);
                MenuManager.Instance.HUDMenu.UpdateHealth(Health, MaxHealth.Value, HealthBarCount);
                StartCoroutine(Die());
                return true;
            }
            
            bool die = base.TakeDamage(Health, true);
            MenuManager.Instance.HUDMenu.UpdateHealth(Health, MaxHealth.Value, HealthBarCount);
            return die;
        }

        public override IEnumerator Die()
        {
            yield return new WaitForSeconds(0.25f);
            //Debug.Break();
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
            //TODO: check if the power up is unlocked and use it ! 
        }

        public override void OnUpdate()
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