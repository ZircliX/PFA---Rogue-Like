using RogueLike.Player;
using UnityEngine;

namespace DeadLink.PowerUpSystem.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/DashPowerUp", fileName = "DashPowerUp")]
    public class DashPowerUp : CooldownPowerUp
    {
        [field: SerializeField] public int BonusDashCount { get; private set; } = 1;

        public override void OnBeUnlocked(RogueLike.Entities.Player player, PlayerMovement playerMovement)
        {
            playerMovement.AddBonusDash(BonusDashCount);
            Debug.Log("Visitor accepted in DeshComponent");
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