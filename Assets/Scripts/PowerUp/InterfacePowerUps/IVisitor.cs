using UnityEngine;

namespace DeadLink.PowerUp.InterfacePowerUps
{
    public interface IVisitor
    {
        public void Visit(DoubleDashComponent component);
        public void Visit(DoubleJumpComponent component);
    }

    public class PowerUp : ScriptableObject, IVisitor
    {
        public int JumpBonus = 1;
        public int DashBonus = 1;
        
        public void Visit(DoubleDashComponent component)
        {
            component.doubleDash += DashBonus;
        }

        public void Visit(DoubleJumpComponent component)
        {
            component.doubleJump += JumpBonus;
        }
    }
}