using RogueLike.Player;
using UnityEngine;

namespace DeadLink.PowerUpSystem.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/BonusHealthBarPowerUp", fileName = "BonusHealthBarPowerUp")]
    public class BonusHealthBarPowerUp : PowerUp
    {
        [field: SerializeField] public int BonusHealthBarCount { get; private set; } = 1;

        public override void OnBeUnlocked(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            if (playerEntity.HealthBarCount >= 3)
            {
                playerEntity.SetFullHealth();
            }
            else
            {
                playerEntity.SetBonusHealthBarCount(BonusHealthBarCount);
            }

            Debug.Log("Visitor accepted in HealthComponent");
        }

        public override void OnBeUsed(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
        }

        public override void OnFinishedToBeUsed(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
        }
        
        public override void OnReset(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            OnFinishedToBeUsed(playerEntity, playerMovement);
        }
    }
}