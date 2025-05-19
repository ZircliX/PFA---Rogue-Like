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
            RogueLike.Entities.PlayerEntity.OnPlayerLastChanceSaved += OnBeUsed;
        }
        
        private void OnDisable()
        {
            RogueLike.Entities.PlayerEntity.OnPlayerLastChanceSaved -= OnBeUsed;

        }
        
        public override void OnBeUnlocked(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            IsUnlocked = true;
            CanBeUsed = true;
            playerEntity.isLastChanceActivated = true;
        }

        public override void OnBeUsed(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            playerEntity.isLastChanceActivated = false;
            playerEntity.StartCoroutine(Cooldown());
        }

        public override void OnFinishedToBeUsed(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            
        }
    }
}