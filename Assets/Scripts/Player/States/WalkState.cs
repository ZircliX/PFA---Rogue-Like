using UnityEngine;

namespace RogueLike.Player.States
{
    [CreateAssetMenu(menuName = "RogueLike/Movement/Walk")]
    public class WalkState : MoveState
    {
        public override Vector3 GetVelocity(PlayerMovement movement, float deltaTime, ref float gravityScale)
        {
            gravityScale = 0;
            Vector3 velocity = base.GetVelocity(movement, deltaTime, ref gravityScale);

            const float snapForce = 2;

            velocity += movement.Gravity.Value.normalized * snapForce * deltaTime;
            
            return velocity;
        }
        
        public override void Enter(PlayerMovement movement)
        {
            movement.PlayerHeight.Write(this, (movement.BaseCapsuleHeight, movement.BaseHeadHeight));
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

        public override (float, float) GetHeight(PlayerMovement movement)
        {
            return (movement.BaseCapsuleHeight, movement.BaseHeadHeight);
        }

        public override MovementState State => MovementState.Walking;
    }
}