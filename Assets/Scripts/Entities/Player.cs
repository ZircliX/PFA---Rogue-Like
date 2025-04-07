using System;
using DeadLink.Entities;
using DeadLink.Entities.Data;
using Enemy;
using LTX.ChanneledProperties;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RogueLike.Player
{
    public class Player : Entity
    {
        [field : SerializeField] public Transform SpawnPosition { get; private set; }
        
        public int Kills { get; private set; }
        
        public override void Spawn(EntityData data, DifficultyData difficultyData, Vector3 spawnPoint)
        {
            base.Spawn(data, difficultyData, spawnPoint);
            
            MaxHealth.AddInfluence(difficultyData, difficultyData.PlayerHealthMultiplier, Influence.Multiply);
            Strength.AddInfluence(difficultyData, difficultyData.PlayerStrengthMultiplier, Influence.Multiply);
            
            SetFullHealth();
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
            if (context.performed)
            {
                Shoot();
            }
        }
    }
}