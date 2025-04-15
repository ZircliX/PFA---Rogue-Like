using DeadLink.PowerUp.Components;
using RogueLike.Player;
using UnityEngine;

namespace DeadLink.PowerUp.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/DashPowerUp", fileName = "DashPowerUp")]

    public class DashPowerUp : PowerUp
    {
        [field: SerializeField] public int BonusDashCount { get; private set; } = 1;
        public override string Name { get; set; } = "DashPowerUp";


        public override void OnBeUnlocked(VisitableComponent visitable)
        {
            PlayerMovement playerMovement = visitable as PlayerMovement;
            if (playerMovement != null)
            {
                playerMovement.AddBonusDash(BonusDashCount);
                Debug.Log("Visitor accepted in DeshComponent");
            }
        }

        public override void OnBeUsed(VisitableComponent visitable)
        {
        }
    }
}