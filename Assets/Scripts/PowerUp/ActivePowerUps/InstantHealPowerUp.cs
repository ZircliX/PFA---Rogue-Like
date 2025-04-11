using DeadLink.PowerUp.Components;
using UnityEngine;

namespace DeadLink.PowerUp.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/InstantHealPowerUp", fileName = "InstantHealPowerUp")]

    public class InstantHealPowerUp : PowerUp
    {
        [field: SerializeField] public int InstantHealBonus { get; private set; } = 40;

        public override void OnBeUnlocked(VisitableComponent visitable)
        {
            isUnlocked = true;
        }

        public override void OnBeUsed(VisitableComponent visitable)
        {
            HealthComponent healthComponent = visitable as HealthComponent;
            if (healthComponent != null)
            {
                if (isUnlocked)
                {
                    healthComponent.health += InstantHealBonus;
                    Debug.Log("Visitor accepted in HealthComponent");
                }
                else
                {
                    Debug.Log("PowerUp is not unlocked");
                }
            }
            
        }
    }
}