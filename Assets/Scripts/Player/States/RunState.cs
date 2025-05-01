using UnityEngine;

namespace RogueLike.Player.States
{
    [CreateAssetMenu(menuName = "RogueLike/Movement/Run")]
    public class RunState : WalkState
    {
        public override MovementState GetNextState(PlayerMovement movement)
        {
            //Debug.Log(movement.WantsToSlide);
            //Debug.Log($"grounded = {movement.IsGrounded}");
            
            if (!movement.IsGrounded)
            {
                return MovementState.Falling;
            }
            if (movement.CrouchInput)
            {
                return MovementState.Crouching;
            }
            if (movement.CanJump())
            {
                return MovementState.Jumping;
            }
            if (movement.CanSlide())
            {
                return MovementState.Sliding;
            }
            if (movement.CanDash())
            {
                return MovementState.Dashing;
            }
            if (movement.InputDirection.sqrMagnitude < PlayerMovement.MIN_THRESHOLD)
            {
                return MovementState.Idle;
            }
            if (movement.WalkInput)
            {
                return MovementState.Walking;
            }

            return State;
        }

        public override MovementState State => MovementState.Running;
    }
}