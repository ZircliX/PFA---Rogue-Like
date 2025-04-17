using System;
using System.Collections.Generic;
using DeadLink.Entities;
using DeadLink.Entities.Data;
using DeadLink.PowerUp;
using Enemy;
using LTX.ChanneledProperties;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RogueLike.Entities
{
    public class Player : Entity
    {
        [field : SerializeField] public Transform SpawnPosition { get; private set; }
        private Dictionary<string, IVisitor> unlockedPowerUps;
        public int Kills { get; private set; }
        
        public override void Spawn(EntityData data, DifficultyData difficultyData, Vector3 spawnPoint)
        {
            base.Spawn(data, difficultyData, spawnPoint);
            HealthBarCount.AddInfluence(difficultyData, difficultyData.PlayerHealthBarAmountMultiplier, Influence.Add);
            MaxHealth.AddInfluence(difficultyData, difficultyData.PlayerHealthMultiplier, Influence.Multiply);
            Strength.AddInfluence(difficultyData, difficultyData.PlayerStrengthMultiplier, Influence.Multiply);
            
            unlockedPowerUps = new Dictionary<string, IVisitor>();
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
                currentShootTime = CurrentWeapon.WeaponData.ShootRate;
            }
            else
            {
                isShooting = false;
            }
        }

        public override void Die()
        {
            base.Die();
            
            
        }

        public override void Unlock(IVisitor visitor)
        {
            visitor.OnBeUnlocked(this);
            unlockedPowerUps.Add(visitor.Name, visitor);
        }

        public override void Use(string name)
        {
            unlockedPowerUps[name].OnBeUsed(this);
        }
    }
}