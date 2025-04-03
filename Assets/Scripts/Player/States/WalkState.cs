using UnityEngine;

namespace RogueLike.Player.States
{
    [CreateAssetMenu(menuName = "RogueLike/Movement/Walk")]
    public class WalkState : MoveState
    {
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
            if (movement.crouchInput)
            {
                return MovementState.Crouching;
            }
            if (movement.runInput)
            {
                return MovementState.Running;
            }
            if (movement.InputDirection.sqrMagnitude < PlayerMovement.MIN_THRESHOLD)
            {
                return MovementState.Idle;
            }

            return State;
        }

        public override MovementState State => MovementState.Walking;
    }
}