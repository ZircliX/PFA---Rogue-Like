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
            playerMovement.AddBonusDash(BonusDashCount);
            Debug.Log("Visitor accepted in DeshComponent");
        }

        public override void OnBeUsed(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
        }

        public override void OnFinishedToBeUsed(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
        }
    }
}