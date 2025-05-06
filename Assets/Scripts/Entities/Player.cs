using System.Collections.Generic;
using DeadLink.Entities;
using DeadLink.Entities.Data;
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
        public bool isInvisible;

        #region Event Functions
        
        private void OnValidate() => this.ValidateRefs();
        
        private void Start()
        {
            if (PowerUpsInputName.Count < 9) return;

            inputToPowerUpName = new Dictionary<string, string>();
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
            
            unlockedPowerUps = new Dictionary<string, IVisitor>();
            inputToPowerUpName = new Dictionary<string, string>();
        }

        protected override void Attack()
        {
            Shoot();
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            LevelManager.Instance.HUDMenuHandler.UpdateHealth(Health, MaxHealth.Value, HealthBarCount.Value);
        }

        public override void Die()
        {

        }
        
        #endregion
        
        protected override void ChangeWeapon(int direction)
        {
            base.ChangeWeapon(direction);
            
            LevelManager.Instance.HUDMenuHandler.ChangeWeapon(currentWeaponIndex);
            LevelManager.Instance.HUDMenuHandler.UpdateAmmunitions(CurrentWeapon.CurrentMunitions, CurrentWeapon.WeaponData.MaxAmmunition);
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
        
        public void StartCoroutine(CooldownPowerUp cooldownPowerUp)
        {
            StartCoroutine(cooldownPowerUp.Cooldown());
        }
        
        #endregion
    }
}