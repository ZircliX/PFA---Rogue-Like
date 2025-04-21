using DeadLink.PowerUpSystem.InterfacePowerUps;
using UnityEngine;

namespace DeadLink.PowerUpSystem
{
    public abstract class PowerUp : ScriptableObject, IVisitor
    {
        [field : SerializeField] public VisitableType VisitableType { get; private set; }
        [field : SerializeField] public string Name { get; private set; }
        [field : SerializeField] public string Description { get; private  set; }
        [field : SerializeField] public Sprite Icon { get; private set; }

        public bool IsUnlocked { get; protected set; }

        public abstract void OnBeUnlocked(IVisitable visitable);
        public abstract void OnBeUsed(IVisitable visitable);
    }
}