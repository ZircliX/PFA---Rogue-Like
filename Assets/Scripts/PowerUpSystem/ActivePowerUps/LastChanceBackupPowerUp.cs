using System;
using RogueLike.Player;
using UnityEngine;

namespace DeadLink.PowerUpSystem.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/LastChanceBackupPowerUp", fileName = "LastChanceBackupPowerUp")]
    public class LastChanceBackupPowerUp : CooldownPowerUp
    {
        private void OnEnable()
        {
            RogueLike.Entities.PlayerEntity.OnPlayerLastChanceUsed += OnBeUsed;
        }
        
        private void OnDisable()
        {
            RogueLike.Entities.PlayerEntity.OnPlayerLastChanceUsed -= OnBeUsed;

        }
        
        public override void OnBeUnlocked(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            base.OnBeUnlocked(playerEntity, playerMovement); 
            IsUnlocked = true;
            CanBeUsed = true;
            playerEntity.isLastChanceActivated = true;
        }

        public override void OnBeUsed(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            if (!CanBeUsed) return;
            
            CanBeUsed = false;
            playerEntity.isLastChanceActivated = false;
            playerEntity.StartCoroutine(CompetenceDuration(playerEntity, playerMovement, OnFinishedToBeUsed));
            //Alors pour ce power up particulierement, le CompetenceDuration est enfait le cooldown

        }

        public override void OnFinishedToBeUsed(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            playerEntity.isLastChanceActivated = true;
            CanBeUsed = true;
        }
        
        public override void OnReset(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            IsUnlocked = false;
            CanBeUsed = false;
            playerEntity.isLastChanceActivated = false;
        }
    }
}