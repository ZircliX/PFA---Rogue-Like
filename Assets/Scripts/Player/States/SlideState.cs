using UnityEngine;

namespace RogueLike.Player.States
{
    [CreateAssetMenu(menuName = "RogueLike/Movement/Slide")]
    public class SlideState : MovementStateBehavior
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

        public override MovementState State => MovementState.Sliding;
    }
}