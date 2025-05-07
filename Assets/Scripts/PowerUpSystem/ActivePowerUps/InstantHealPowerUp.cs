using RogueLike.Player;
using UnityEngine;

namespace DeadLink.PowerUpSystem.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/InstantHealPowerUp", fileName = "InstantHealPowerUp")]
    public class InstantHealPowerUp : CooldownPowerUp
    {
        [field: SerializeField] public int InstantHealBonus { get; private set; } = 20;

        public override void OnBeUnlocked(RogueLike.Entities.Player player, PlayerMovement playerMovement)
        {
            IsUnlocked = true;
            CanBeUsed = true;
            
        }

        public override void OnBeUsed(RogueLike.Entities.Player player, PlayerMovement playerMovement)
        {
            if (IsUnlocked && CanBeUsed)
            {
                player.SetInstantHeal(InstantHealBonus);
                player.StartCoroutine(Cooldown());
            }
        }

        public override void OnFinishedToBeUsed(RogueLike.Entities.Player player, PlayerMovement playerMovement)
        {
        }
    }
}