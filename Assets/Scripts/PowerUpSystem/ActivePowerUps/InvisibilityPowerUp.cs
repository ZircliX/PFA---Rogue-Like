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
                player.isInvisible = true;
                player.StartCoroutine(Cooldown());
                
            }
            else
            {
                Debug.Log("PowerUp is not unlocked or already used");
            }
        }

        public override void OnFinishedToBeUsed(RogueLike.Entities.Player player, PlayerMovement playerMovement)
        {
        }
    }
}