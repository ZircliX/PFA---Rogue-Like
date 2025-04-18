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
            if (movement.WantsToJump)
            {
                return MovementState.Jumping;
            }
            if (movement.WantsToSlide)
            {
                return MovementState.Sliding;
            }
            /*
            if (movement.IsWalled && movement.CurrentWall != null)
            {
                return MovementState.WallRunning;
            }
            */
            if (movement.InputDirection.sqrMagnitude < PlayerMovement.MIN_THRESHOLD)
            {
                return MovementState.Idle;
            }
            if (!movement.RunInput)
            {
                return MovementState.Walking;
            }

            return State;
        }

        public override MovementState State => MovementState.Running;
    }
}