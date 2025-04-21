using System.Collections.Generic;
using DeadLink.Entities;
using DeadLink.Entities.Data;
using DeadLink.PowerUpSystem.InterfacePowerUps;
using Enemy;
using KBCore.Refs;
using LTX.ChanneledProperties;
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

        private void OnValidate() => this.ValidateRefs();
        
        private void Start()
        {
            inputToPowerUpName = new Dictionary<string, string>()
            {
                { PowerUpsInputName[0], GameMetrics.Global.InstantHealPowerUp.Name },
                { PowerUpsInputName[1], GameMetrics.Global.WallHackPowerUp.Name },
            };
        }

        public override void Spawn(EntityData data, DifficultyData difficultyData, Vector3 spawnPoint)
        {
            base.Spawn(data, difficultyData, spawnPoint);
            
            HealthBarCount.AddInfluence(difficultyData, difficultyData.PlayerHealthBarAmountMultiplier, Influence.Add);
            MaxHealth.AddInfluence(difficultyData, difficultyData.PlayerHealthMultiplier, Influence.Multiply);
            Strength.AddInfluence(difficultyData, difficultyData.PlayerStrengthMultiplier, Influence.Multiply);
            
            unlockedPowerUps = new Dictionary<string, IVisitor>();
            inputToPowerUpName = new Dictionary<string, string>();
        }

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
                Debug.Log("Reloading input");
                Reload();
            }
        }
        public override void UsePowerUp(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            string actionName = context.action.name;
            
            if (inputToPowerUpName.TryGetValue(actionName, out string powerUpName))
            {
                if (unlockedPowerUps.TryGetValue(powerUpName, out IVisitor powerUp))
                {
                    powerUp.OnBeUsed(this, pm);
                }
            }
        }

        public override void Unlock(IVisitor visitor)
        {
            visitor.OnBeUnlocked(this, pm);
            unlockedPowerUps.Add(visitor.Name, visitor);
        }
    }
}