using UnityEngine;

namespace RogueLike.Player.States
{
    [CreateAssetMenu(menuName = "RogueLike/Movement/Run")]
    public class RunState : WalkState
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
            if (movement.WantsToSlide)
            {
                return MovementState.Sliding;
            }
            if (movement.InputDirection.sqrMagnitude < PlayerMovement.MIN_THRESHOLD)
            {
                return MovementState.Idle;
            }
            if (!movement.runInput)
            {
                return MovementState.Walking;
            }

            return State;
        }

        public override MovementState State => MovementState.Running;
    }
}