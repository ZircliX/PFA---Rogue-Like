using RogueLike.Entities;
using RogueLike.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DeadLink.Player
{
    public class PlayerPowerUps : MonoBehaviour
    {
        
        
        public void ReadInputPowerUp1(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (LevelManager.Instance.PlayerController.PlayerEntity.PowerUps[0] != null)
                {
                    LevelManager.Instance.PlayerController.PlayerEntity.PowerUps[0].OnBeUsed(LevelManager.Instance.PlayerController.PlayerEntity,LevelManager.Instance.PlayerController.PlayerMovement);
                }
            }
        }
        
        public void ReadInputPowerUp2(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (LevelManager.Instance.PlayerController.PlayerEntity.PowerUps[1] != null)
                {
                    LevelManager.Instance.PlayerController.PlayerEntity.PowerUps[1].OnBeUsed(LevelManager.Instance.PlayerController.PlayerEntity,LevelManager.Instance.PlayerController.PlayerMovement);
                }
            }
        }
        
        public void ReadInputPowerUp3(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (LevelManager.Instance.PlayerController.PlayerEntity.PowerUps[2] != null)
                {
                    LevelManager.Instance.PlayerController.PlayerEntity.PowerUps[2].OnBeUsed(LevelManager.Instance.PlayerController.PlayerEntity,LevelManager.Instance.PlayerController.PlayerMovement);
                }
            }
        }
    }
}