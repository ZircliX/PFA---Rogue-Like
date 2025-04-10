using DeadLink.PowerUp.Components;
using UnityEngine;

namespace DeadLink.PowerUp
{
    public abstract class PowerUp : ScriptableObject, IVisitor
    {
        public abstract void Visit(VisitableComponent visitable);
    }
}