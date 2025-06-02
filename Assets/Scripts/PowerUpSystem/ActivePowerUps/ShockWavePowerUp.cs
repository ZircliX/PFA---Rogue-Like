using RogueLike.Player;
using UnityEngine;

namespace DeadLink.PowerUpSystem.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/ShockWavePowerUp", fileName = "ShockWavePowerUp")]
    public class ShockWavePowerUp : CooldownPowerUp
    {
        public override void OnBeUnlocked(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            base.OnBeUnlocked(playerEntity, playerMovement); 
        }

        public override void OnBeUsed(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
        }

        public override void OnFinishedToBeUsed(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
        }
    }
}