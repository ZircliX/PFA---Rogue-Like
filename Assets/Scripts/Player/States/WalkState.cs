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
            if (movement.CrouchInput)
            {
                return MovementState.Crouching;
            }
            if (movement.RunInput)
            {
                return MovementState.Running;
            }
            /*
            if (movement.IsWalled && movement.CurrentWall != null)
            {
                return MovementState.WallRunning;
            }
            */
            if (movement.InputDirection.sqrMagnitude < PlayerMovement.MIN_THRESHOLD)
            {
                //Debug.Log("HAAAAAAA");
                return MovementState.Idle;
            }

            return State;
        }

        public override MovementState State => MovementState.Walking;
    }
}