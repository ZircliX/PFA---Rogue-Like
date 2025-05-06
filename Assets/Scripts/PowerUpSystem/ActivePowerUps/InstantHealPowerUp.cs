using RogueLike.Player;
using UnityEngine;

namespace DeadLink.PowerUpSystem.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/InstantHealPowerUp", fileName = "InstantHealPowerUp")]
    public class InstantHealPowerUp : CooldownPowerUp
    {
        [field: SerializeField] public int InstantHealBonus { get; private set; } = 40;

        public override void OnBeUnlocked(RogueLike.Entities.Player player, PlayerMovement playerMovement)
        {
            IsUnlocked = true;
        }

        public override void OnBeUsed(RogueLike.Entities.Player player, PlayerMovement playerMovement)
        {
            if (IsUnlocked)
            {
                player.SetInstantHeal(InstantHealBonus);
                Debug.Log("Visitor accepted in HealthComponent");
            }
            else
            {
                Debug.Log("PowerUp is not unlocked");
            }
        }

        public override void OnFinishedToBeUsed(RogueLike.Entities.Player player, PlayerMovement playerMovement)
        {
            throw new System.NotImplementedException();
        }
    }
}