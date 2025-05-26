using RogueLike.Player;
using UnityEngine;
using UnityEngine.VFX;

namespace DeadLink.PowerUpSystem.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/CounterPowerUp", fileName = "CounterPowerUp")]
    public class CounterPowerUp : PowerUp
    {
        // MAYBE LEAVE THIS
        public override void OnBeUnlocked(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            base.OnBeUnlocked(playerEntity, playerMovement);    
            IsUnlocked = true;
            CanBeUsed = true;
        }

        public override void OnBeUsed(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
        }

        public override void OnFinishedToBeUsed(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
        }
    }
}