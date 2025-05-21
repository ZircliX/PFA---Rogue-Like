using DeadLink.PowerUpSystem.InterfacePowerUps;
using RogueLike;
using UnityEngine.InputSystem;

namespace DeadLink.Entities.Enemies
{
    public class DistanceEnemy : Enemy
    {
        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void Unlock(IVisitor visitor)
        {
        }

        public override void UsePowerUp(InputAction.CallbackContext context)
        {
        }
        
    }
}