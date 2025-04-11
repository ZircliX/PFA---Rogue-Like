using UnityEngine;

namespace DeadLink.PowerUp.Components
{
    public class HealthComponent: VisitableComponent
    {
        public int healthBarCount = 3;
        public int health = 300;

        public override void Unlock(IVisitor visitor)
        {
            visitor.OnBeUnlocked(this);
        }

        public override void Use(IVisitor visitor)
        {
            visitor.OnBeUsed(this);
        }

        public void UseInstantHeal()
        {
            
        }
    }
}