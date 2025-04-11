using UnityEngine;

namespace DeadLink.PowerUp.Components
{
    public class JumpComponent : VisitableComponent
    {
        public int RemainingJump = 1;

        public override void Unlock(IVisitor visitor)
        {
            Debug.Log("Accepting visitor in JumpComponent");
            visitor.OnBeUnlocked(this);
        }

        public override void Use(IVisitor visitor)
        {
        }
    }
}