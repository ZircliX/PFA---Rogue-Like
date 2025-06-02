using RogueLike.Player;
using UnityEngine;

namespace DeadLink.PowerUpSystem.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/FastFallPowerUp", fileName = "FastFallPowerUp")]
    public class QuickFallPowerUp : CooldownPowerUp
    {
        public override void OnBeUnlocked(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            base.OnBeUnlocked(playerEntity, playerMovement);
            IsUnlocked = true;
            CanBeUsed = true;
        }

        public override void OnBeUsed(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            if (IsUnlocked && CanBeUsed)
            {
                CanBeUsed = false;
                playerMovement.ActiveQuickFall();
                playerMovement.StartCoroutine(Cooldown());
                playerMovement.StartCoroutine(CompetenceDuration(playerEntity, playerMovement, OnFinishedToBeUsed));
            }
        }

        public override void OnFinishedToBeUsed(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            playerMovement.DesactiveQuickFall();
        }
    }
}