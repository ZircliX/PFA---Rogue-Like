using UnityEngine;

namespace DeadLink.PowerUp.Components
{
    public abstract class VisitableComponent : MonoBehaviour, IVisitable
    {
        public abstract void Accept(IVisitor visitor);
    }
}