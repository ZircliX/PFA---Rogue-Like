using RogueLike.Player;
using UnityEngine;

namespace DeadLink.PowerUpSystem.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/DashPowerUp", fileName = "DashPowerUp")]
    public class DashPowerUp : PowerUp
    {
        [field: SerializeField] public int BonusDashCount { get; private set; } = 1;

        public override void OnBeUnlocked(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            base.OnBeUnlocked(playerEntity, playerMovement); 
            IsUnlocked = true;
            OnBeUsed(playerEntity, playerMovement);
        }

        public override void OnBeUsed(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            if (IsUnlocked)
            {
                playerMovement.AddBonusDash(BonusDashCount);
                OnFinishedToBeUsed(playerEntity, playerMovement);
            }
        }

        public override void OnFinishedToBeUsed(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
        }
        
        public override void OnReset(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            OnFinishedToBeUsed(playerEntity, playerMovement);
            IsUnlocked = false;
        }
    }
}