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
        }

        public override void Exit(PlayerMovement movement)
        {
            base.Exit(movement);
        }

        public override MovementState GetNextState(PlayerMovement movement)
        {
            if (!movement.IsGrounded)
            {
                return MovementState.Falling;
            }
            if (!movement.CrouchInput)
            {
                return MovementState.Idle;
            }

            return State;
        }

        public override MovementState State => MovementState.Crouching;
    }
}