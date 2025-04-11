using UnityEngine;

namespace DeadLink.PowerUp.Components
{
    public abstract class VisitableComponent : MonoBehaviour, IVisitable
    {
        public abstract void Unlock(IVisitor visitor);
        public abstract void Use(IVisitor visitor);
    }
}