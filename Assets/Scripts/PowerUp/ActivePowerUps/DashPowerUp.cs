using DeadLink.PowerUp.Components;
using UnityEngine;

namespace DeadLink.PowerUp.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/DashPowerUp", fileName = "DashPowerUp")]

    public class DashPowerUp : PowerUp
    {
        [field: SerializeField] public int BonusDashCount { get; private set; } = 1;

        public override void OnBeUnlocked(VisitableComponent visitable)
        {
            DashComponent dashComponent = visitable as DashComponent;
            if (dashComponent != null)
            {
                dashComponent.remainingDash += BonusDashCount;
                Debug.Log("Visitor accepted in DeshComponent");
            }
        }

        public override void OnBeUsed(VisitableComponent visitable)
        {
            throw new System.NotImplementedException();
        }
    }
}