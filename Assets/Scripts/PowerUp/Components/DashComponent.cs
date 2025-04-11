using UnityEngine;

namespace DeadLink.PowerUp.Components
{
    public class DashComponent : VisitableComponent
    {
        public int remainingDash = 1;
        
        public override void Unlock(IVisitor visitor)
        {
            visitor.OnBeUnlocked(this);
        }

        public override void Use(IVisitor visitor)
        {
        }
    }
}