using UnityEngine;

namespace RogueLike.Player
{
    public abstract class MovementStateBehavior : ScriptableObject
    {
        public abstract void Initialize(PlayerMovement movement);
        public abstract void Dispose(PlayerMovement movement);

        public abstract void Enter(PlayerMovement movement);
        public abstract void Exit(PlayerMovement movement);
        public abstract Vector3 GetVelocity(PlayerMovement movement, float deltaTime);
        
        public abstract MovementState State { get; }
    }
}