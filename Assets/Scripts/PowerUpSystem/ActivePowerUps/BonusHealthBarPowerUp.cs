using RogueLike.Player;
using UnityEngine;

namespace DeadLink.PowerUpSystem.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/BonusHealthBarPowerUp", fileName = "BonusHealthBarPowerUp")]
    public class BonusHealthBarPowerUp : PowerUp
    {
        [field: SerializeField] public int BonusHealthBarCount { get; private set; } = 1;

        public override void OnBeUnlocked(RogueLike.Entities.Player player, PlayerMovement playerMovement)
        {
            if (player.HealthBarCount >= 3)
            {
                player.SetFullHealth();
            }
            else
            {
                player.SetBonusHealthBarCount(BonusHealthBarCount);
            }

            Debug.Log("Visitor accepted in HealthComponent");
        }

        public override void OnBeUsed(RogueLike.Entities.Player player, PlayerMovement playerMovement)
        {
        }

        public override void OnFinishedToBeUsed(RogueLike.Entities.Player player, PlayerMovement playerMovement)
        {
            throw new System.NotImplementedException();
        }
    }
}