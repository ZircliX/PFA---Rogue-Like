using DeadLink.PowerUp.Components;
using UnityEngine;

namespace DeadLink.PowerUp
{
    public abstract class PowerUp : ScriptableObject, IVisitor
    {
        public bool isUnlocked { get; protected set; }
        public abstract string Name { get; set; }

        public abstract void OnBeUnlocked(VisitableComponent visitable);
        public abstract void OnBeUsed(VisitableComponent visitable);
    }
}