using System;
using System.Collections.Generic;
using DeadLink.Entities;
using DeadLink.Entities.Data;
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
            
            VisitableReferenceManager.Instance.RegisterComponent(VisitableType.Player, this);
            
            HealthBarCount.AddInfluence(difficultyData, difficultyData.PlayerHealthBarAmountMultiplier, Influence.Add);
            MaxHealth.AddInfluence(difficultyData, difficultyData.PlayerHealthMultiplier, Influence.Multiply);
            Strength.AddInfluence(difficultyData, difficultyData.PlayerStrengthMultiplier, Influence.Multiply);
            
            unlockedPowerUps = new Dictionary<string, IVisitor>();
            inputToPowerUpName = new Dictionary<string, string>();
        }

        private void OnEnable()
        {
            VisitableReferenceManager.Instance.RegisterComponent(GameMetrics.Global.PlayerVisitableType, this);
        }

        private void OnDisable()
        {
            if (!VisitableReferenceManager.HasInstance) return;
            VisitableReferenceManager.Instance.UnregisterComponent(GameMetrics.Global.PlayerVisitableType);
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
                    //TODO: c'est de la D (à rework)
                    if (VisitableReferenceManager.Instance.Components.TryGetValue(VisitableType.Player, out var pvisitable))
                    {
                        powerUp.OnBeUsed(pvisitable);
                    }
                    else if (VisitableReferenceManager.Instance.Components.TryGetValue(VisitableType.PlayerMovement, out var pmvisitable))
                    {
                        powerUp.OnBeUsed(pmvisitable);
                    }
                    
                }
            }
        }

        public override void Unlock(IVisitor visitor)
        {
            visitor.OnBeUnlocked(this);
            unlockedPowerUps.Add(visitor.Name, visitor);
        }
    }
}