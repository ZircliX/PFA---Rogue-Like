using UnityEngine;

namespace DeadLink.PowerUp.Components
{
    public class JumpComponent : VisitableComponent
    {
        public int RemainingJump = 1;

        public override void Accept(IVisitor visitor)
        {
            Debug.Log("Accepting visitor in JumpComponent");
            visitor.Visit(this);
        }
    }
}