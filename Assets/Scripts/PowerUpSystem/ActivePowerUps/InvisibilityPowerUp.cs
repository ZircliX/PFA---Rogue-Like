using RogueLike.Player;
using UnityEngine;

namespace DeadLink.PowerUpSystem.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/InvisibilityPowerUp", fileName = "InvisibilityPowerUp")]
    public class InvisibilityPowerUp : CooldownPowerUp
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
                player.ActiveInvisibility();
                player.StartCooldownCoroutine(this);
                player.StartCoroutine(CompetenceDuration(player, playerMovement, OnFinishedToBeUsed));
            }
        }

        public override void OnFinishedToBeUsed(RogueLike.Entities.Player player, PlayerMovement playerMovement)
        {
            player.DesactiveInvisibility();
        }
    }
}