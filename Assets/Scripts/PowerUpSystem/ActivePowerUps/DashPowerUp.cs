using DeadLink.PowerUpSystem.InterfacePowerUps;
using RogueLike.Player;
using UnityEngine;

namespace DeadLink.PowerUpSystem.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/DashPowerUp", fileName = "DashPowerUp")]

    public class DashPowerUp : PowerUp
    {
        [field: SerializeField] public int BonusDashCount { get; private set; } = 1;

        public override void OnBeUnlocked(IVisitable visitable)
        {
            PlayerMovement playerMovement = visitable as PlayerMovement;
            if (playerMovement != null)
            {
                playerMovement.AddBonusDash(BonusDashCount);
                Debug.Log("Visitor accepted in DeshComponent");
            }
        }

        public override void OnBeUsed(IVisitable visitable)
        {
        }
    }
}