using RogueLike.Player;
using UnityEngine;

namespace DeadLink.PowerUpSystem.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/InvisibilityPowerUp", fileName = "InvisibilityPowerUp")]
    public class InvisibilityPowerUp : CooldownPowerUp
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
                playerEntity.ActiveInvisibility();
                playerEntity.StartCooldownCoroutine(this);
                playerEntity.StartCoroutine(CompetenceDuration(playerEntity, playerMovement, OnFinishedToBeUsed));
            }
        }

        public override void OnFinishedToBeUsed(RogueLike.Entities.PlayerEntity playerEntity,
            PlayerMovement playerMovement)
        {
            playerEntity.DesactiveInvisibility();
        }

        public override void OnReset(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            OnFinishedToBeUsed(playerEntity, playerMovement);
            IsUnlocked = false;
        }
    }
}