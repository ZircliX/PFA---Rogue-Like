using DeadLink.PowerUp.Components;
using UnityEngine;

namespace DeadLink.PowerUp
{
    public abstract class PowerUp : ScriptableObject, IVisitor
    {
        public bool isUnlocked;

        public abstract void OnBeUnlocked(VisitableComponent visitable);
        public abstract void OnBeUsed(VisitableComponent visitable);
    }
}