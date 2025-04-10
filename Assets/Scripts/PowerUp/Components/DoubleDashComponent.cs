using DeadLink.PowerUp.InterfacePowerUps;
using UnityEngine;


namespace DeadLink.PowerUp.InterfacePowerUps
{
    public class DoubleDashComponent : MonoBehaviour, IVisitable
    {
        public int doubleDash = 1;

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
            
        }
    }
}