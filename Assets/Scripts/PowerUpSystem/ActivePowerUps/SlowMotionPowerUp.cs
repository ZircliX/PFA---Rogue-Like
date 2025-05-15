using RogueLike.Player;
using UnityEngine;

namespace DeadLink.PowerUpSystem.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/SlowMotionPowerUp", fileName = "SlowMotionPowerUp")]

    public class SlowMotionPowerUp : CooldownPowerUp
    {
        public override void OnBeUnlocked(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            IsUnlocked = true;
            CanBeUsed = true;
        }

        public override void OnBeUsed(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            if (IsUnlocked && CanBeUsed)
            {
                CanBeUsed = false;
                playerMovement.StartCoroutine(Cooldown());
                playerMovement.StartCoroutine(CompetenceDuration(playerEntity, playerMovement, OnFinishedToBeUsed));
            }
        }

        public override void OnFinishedToBeUsed(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            
        }
    }
}