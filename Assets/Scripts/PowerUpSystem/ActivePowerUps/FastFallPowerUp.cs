using RogueLike.Player;
using UnityEngine;

namespace DeadLink.PowerUpSystem.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/FastFallPowerUp", fileName = "FastFallPowerUp")]
    public class FastFallPowerUp : CooldownPowerUp
    {
        public override void OnBeUnlocked(RogueLike.Entities.Player player, PlayerMovement playerMovement)
        {
        }

        public override void OnBeUsed(RogueLike.Entities.Player player, PlayerMovement playerMovement)
        {
        }

        public override void OnFinishedToBeUsed(RogueLike.Entities.Player player, PlayerMovement playerMovement)
        {
        }
    }
}