using UnityEngine;

namespace DeadLink.PowerUp.Components
{
    public class DashComponent : VisitableComponent
    {
        public int RemainingDash = 1;
        
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}