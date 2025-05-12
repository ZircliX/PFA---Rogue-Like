using RogueLike.Player;
using UnityEngine;

namespace DeadLink.PowerUpSystem.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/FastFallPowerUp", fileName = "FastFallPowerUp")]
    public class QuickFallPowerUp : CooldownPowerUp
    {
        public override void OnBeUnlocked(RogueLike.Entities.Player player, PlayerMovement playerMovement)
        {
            IsUnlocked = true;
            CanBeUsed = true;
        }

        public override void OnBeUsed(RogueLike.Entities.Player player, PlayerMovement playerMovement)
        {
            if (IsUnlocked && CanBeUsed)
            {
                CanBeUsed = false;
                playerMovement.ActiveQuickFall();
                playerMovement.StartCoroutine(Cooldown());
                playerMovement.StartCoroutine(CompetenceDuration(player, playerMovement, OnFinishedToBeUsed));
            }
        }

        public override void OnFinishedToBeUsed(RogueLike.Entities.Player player, PlayerMovement playerMovement)
        {
            playerMovement.DesactiveQuickFall();
        }
    }
}