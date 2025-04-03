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

                if (movement.crouchInput)
                    nextState = MovementState.Crouching;
                else if (movement.InputDirection.sqrMagnitude > PlayerMovement.MIN_THRESHOLD)
                {
                    nextState = movement.runInput ? MovementState.Running : MovementState.Walking;
                }

                return nextState;
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