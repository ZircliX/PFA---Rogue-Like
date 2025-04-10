using UnityEngine;

namespace RogueLike.Player.States
{
    [CreateAssetMenu(menuName = "RogueLike/Movement/Crouch")]
    public class CrouchState : MoveState
    {
        [Header("Height")]
        [SerializeField] private float headPositionOffset = 0.5f;
        [SerializeField] private float colliderHightOffset = 0.5f;
        public override void Enter(PlayerMovement movement)
        {
            base.Enter(movement);
            
            /*
            movement.Head.position -= Vector3.up * headPositionOffset;
            movement.CapsuleCollider.height -= colliderHightOffset;
            movement.CapsuleCollider.center -= Vector3.up * colliderHightOffset;
            */
        }

        public override void Exit(PlayerMovement movement)
        {
            base.Exit(movement);
            
            /*
            movement.Head.position += Vector3.up * headPositionOffset;
            movement.CapsuleCollider.height += colliderHightOffset;
            movement.CapsuleCollider.center += Vector3.up * colliderHightOffset;
            */
        }

        public override MovementState GetNextState(PlayerMovement movement)
        {
            if (!movement.IsGrounded)
            {
                return MovementState.Falling;
            }
            if (!movement.CrouchInput)
            {
                return MovementState.Idle;
            }

            return State;
        }

        public override MovementState State => MovementState.Crouching;
    }
}