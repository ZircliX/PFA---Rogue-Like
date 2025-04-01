using UnityEngine;

namespace RogueLike.Player
{
    public abstract class MovementStateBehavior : ScriptableObject
    {
        public abstract void Initialize(PlayerMovement movement);
        public abstract void Dispose(PlayerMovement movement);
        
        public abstract void Enter(PlayerMovement movement);
        public abstract void Exit(PlayerMovement movement);
        public abstract void OnFixedUpdate(PlayerMovement movement);
        
        public abstract MovementState State { get; }
    }
}