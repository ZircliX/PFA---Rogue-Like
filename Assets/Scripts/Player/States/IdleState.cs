using UnityEngine;

namespace RogueLike.Player.States
{
    [CreateAssetMenu(menuName = "RogueLike/Movement/Idle")]
    public class IdleState : MovementStateBehavior
    {
        private Camera cam;
        
        public override void Initialize(PlayerMovement movement)
        {
            cam = Camera.main;
        }

        public override void Dispose(PlayerMovement movement)
        {
        }

        public override void Enter(PlayerMovement movement)
        {
            movement.PlayerHeight.Write(this, (movement.BaseCapsuleHeight, movement.BaseHeadHeight));
        }

        public override void Exit(PlayerMovement movement)
        {
        }

        public override Vector3 GetVelocity(PlayerMovement movement, float deltaTime, ref float gravityScale)
        {
            gravityScale = 0;
            return Vector3.zero;
        }

        public override MovementState GetNextState(PlayerMovement movement)
        {
            if (!movement.IsGrounded)
            {
                return MovementState.Falling;
            }
            if (movement.WantsToJump)
            {
                return MovementState.Jumping;
            }
            if (movement.CrouchInput)
            {
                return MovementState.Crouching;
            }
            if (movement.InputDirection.sqrMagnitude > PlayerMovement.MIN_THRESHOLD)
            {
                //Debug.Log("WHUTTTTT");
                return MovementState.Walking;
            }

            return State;
        }

        public override (float, float) GetHeight(PlayerMovement movement)
        {
            return (movement.BaseCapsuleHeight, movement.BaseHeadHeight);
        }

        public override MovementState State => MovementState.Idle;
    }
}