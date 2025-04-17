using DeadLink.PowerUpSystem.InterfacePowerUps;
using UnityEngine;

namespace DeadLink.PowerUpSystem
{
    public abstract class PowerUp : ScriptableObject, IVisitor
    {
        [field : SerializeField] public VisitableType VisitableType { get; private set; }
        public bool IsUnlocked { get; protected set; }
        public abstract string Name { get; set; }
        
        public abstract void OnBeUnlocked(IVisitable visitable);
        public abstract void OnBeUsed(IVisitable visitable);
    }
}