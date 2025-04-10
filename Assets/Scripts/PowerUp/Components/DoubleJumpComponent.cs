using DeadLink.PowerUp.InterfacePowerUps;
using UnityEngine;


namespace DeadLink.PowerUp.InterfacePowerUps
{
    public class DoubleJumpComponent : MonoBehaviour, IVisitable
    {
        public int doubleJump = 1;

        public void Accept(IVisitor visitor)
        {
        }
    }
}