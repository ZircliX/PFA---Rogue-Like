using UnityEngine;

namespace DeadLink.PowerUp.Components
{
    public class HealthComponent: VisitableComponent
    {
        public int health = 300;

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}