using UnityEngine.InputSystem;

namespace DeadLink.PowerUpSystem.InterfacePowerUps
{
    public interface IVisitable
    {
        public void Unlock(IVisitor visitor);
        public void UsePowerUp(InputAction.CallbackContext context);
    }
}