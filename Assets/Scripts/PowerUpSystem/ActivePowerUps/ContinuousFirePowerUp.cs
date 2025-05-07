using RogueLike.Player;
using UnityEngine;

namespace DeadLink.PowerUpSystem.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/ContinuousFirePowerUp", fileName = "ContinuousFirePowerUp")]
    public class ContinuousFirePowerUp : CooldownPowerUp
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
                CanBeUsed = true;
                player.ActiveContinuousFire(this);
                player.StartCoroutine(Cooldown());
                player.StartCoroutine(CompetenceDuration(player, playerMovement, OnFinishedToBeUsed));
            }
        }

        public override void OnFinishedToBeUsed(RogueLike.Entities.Player player, PlayerMovement playerMovement)
        {
            player.DesactiveContinuousFire();
        }
    }
}