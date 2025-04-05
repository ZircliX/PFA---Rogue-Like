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

        public abstract MovementState GetNextState(PlayerMovement movement);

        protected virtual Vector3 GetCameraDirection(PlayerMovement movement, Vector2 direction)
        {
            Camera movementCamera = movement.Camera;
            
            Vector3 worldInputs = movementCamera.transform.right * direction.x;
            float cameraDotProduct = Vector3.Dot(movementCamera.transform.forward, -movement.Gravity.Value.normalized);
            
            worldInputs += cameraDotProduct switch
            {
                < -0.8f => movementCamera.transform.up,
                > 0.8f => -movementCamera.transform.up,
                _ => movementCamera.transform.forward
            } * direction.y;
            
            return worldInputs;
        }
        
        protected virtual Vector3 GetWorldInputs(PlayerMovement movement)
        {
            return GetCameraDirection(movement, new Vector2(movement.InputDirection.x, movement.InputDirection.z));
        }
        
        protected virtual Vector3 GetProjectionPlaneNormal(PlayerMovement movement)
        {
            return movement.GroundNormal;
        }
        
        public abstract MovementState State { get; }
    }
}