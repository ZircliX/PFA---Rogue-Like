using UnityEngine;

namespace RogueLike.Player.States
{
    [CreateAssetMenu(menuName = "RogueLike/Movement/Crouch")]
    public class CrouchState : MoveState
    {
        [SerializeField] private float playerCrouchHeight = 0.5f;
        public override void Enter(PlayerMovement movement)
        {
            base.Enter(movement);
            movement.transform.localScale = new Vector3(1, playerCrouchHeight, 1);
            movement.rb.MovePosition(movement.rb.position - GetProjectionPlaneNormal(movement) * 0.25f);
        }

        public override void Exit(PlayerMovement movement)
        {
            base.Exit(movement);
            movement.transform.localScale = new Vector3(1, 1, 1);
        }

        public override MovementState GetNextState(PlayerMovement movement)
        {
            if (!movement.IsGrounded)
            {
                return MovementState.Falling;
            }
            if (!movement.crouchInput)
            {
                return MovementState.Idle;
            }

            return State;
        }

        public override MovementState State => MovementState.Crouching;
    }
}