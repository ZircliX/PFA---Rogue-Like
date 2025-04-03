using UnityEngine;

namespace RogueLike.Player.States
{
    [CreateAssetMenu(menuName = "RogueLike/Movement/WallRun")]
    public class WallRunState : MovementStateBehavior
    {
        public override void Initialize(PlayerMovement movement)
        {
            
        }

        public override void Dispose(PlayerMovement movement)
        {
        }

        public override void Enter(PlayerMovement movement)
        {
        }

        public override void Exit(PlayerMovement movement)
        {
        }

        public override Vector3 GetVelocity(PlayerMovement movement, float deltaTime)
        {
            return Vector3.zero;
        }

        public override MovementState GetNextState(PlayerMovement movement)
        {
            return MovementState.Falling;
        }

        public override MovementState State => MovementState.WallRuning;
    }
}