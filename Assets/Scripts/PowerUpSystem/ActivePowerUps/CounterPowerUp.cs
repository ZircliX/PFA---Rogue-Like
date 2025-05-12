using RogueLike.Player;
using UnityEngine;
using UnityEngine.VFX;

namespace DeadLink.PowerUpSystem.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/CounterPowerUp", fileName = "CounterPowerUp")]
    public class CounterPowerUp : PowerUp
    {
        
        public override void OnBeUnlocked(RogueLike.Entities.Player player, PlayerMovement playerMovement)
        {
            IsUnlocked = true;
            CanBeUsed = true;
        }

        public override void OnBeUsed(RogueLike.Entities.Player player, PlayerMovement playerMovement)
        {
        }

        public override void OnFinishedToBeUsed(RogueLike.Entities.Player player, PlayerMovement playerMovement)
        {
        }
    }
}