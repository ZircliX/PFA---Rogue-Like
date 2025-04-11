using DeadLink.PowerUp.Components;
using UnityEngine;

namespace DeadLink.PowerUp.ActivePowerUps
{
    
    public class BonusHealthBarPowerUp : PowerUp
    {
        [field: SerializeField] public int BonusHealthBarCount { get; private set; } = 1;

        public override void OnBeUnlocked(VisitableComponent visitable)
        {
            HealthComponent healthComponent = visitable as HealthComponent;
            if (healthComponent != null)
            {
                healthComponent.healthBarCount += BonusHealthBarCount;
                Debug.Log("Visitor accepted in HealthComponent");
            }
        }

        public override void OnBeUsed(VisitableComponent visitable)
        {
        }
    }
}