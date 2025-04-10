using UnityEngine;

namespace RogueLike.Player.States
{
    [CreateAssetMenu(menuName = "RogueLike/Movement/Fall")]
    public class FallState : MoveState
    {
        public override MovementState GetNextState(PlayerMovement movement)
        {
            if (movement.IsGrounded)
            {
                MovementState nextState = MovementState.Idle;

                if (movement.CrouchInput)
                    nextState = MovementState.Crouching;
                else if (movement.InputDirection.sqrMagnitude > PlayerMovement.MIN_THRESHOLD)
                {
                    nextState = movement.RunInput ? MovementState.Running : MovementState.Walking;
                }

                return nextState;
            }
            if (movement.WantsToWallrun)
            {
                return MovementState.WallRunning;
            }
            
            return State;
        }

        protected override Vector3 GetProjectionPlaneNormal(PlayerMovement movement)
        {
            return -movement.Gravity.Value.normalized;
        }
        
        public override MovementState State => MovementState.Falling;
    }
}