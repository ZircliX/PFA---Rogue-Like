using UnityEngine;

namespace DeadLink.PowerUpSystem.InterfacePowerUps
{
    public interface IVisitor
    {
        public string Name { get;}
        public string Description { get;}
        public Sprite Icon { get; }
        public void OnBeUnlocked(IVisitable visitable);
        public void OnBeUsed(IVisitable visitable);
        
    }
}