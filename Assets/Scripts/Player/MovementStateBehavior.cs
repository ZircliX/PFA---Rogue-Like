using DeadLink.Cameras;
using UnityEngine;

namespace RogueLike.Player
{
    public abstract class MovementStateBehavior : ScriptableObject
    {
        protected Camera cam;

        public virtual void Initialize(PlayerMovement movement)
        {
            cam = movement.Camera;
        }
        public abstract void Dispose(PlayerMovement movement);

        public abstract void Enter(PlayerMovement movement);
        public abstract void Exit(PlayerMovement movement);
        public abstract Vector3 GetVelocity(PlayerMovement movement, float deltaTime, ref float gravityScale);

        public abstract MovementState GetNextState(PlayerMovement movement);

        public abstract (float, float) GetHeight(PlayerMovement movement);

        public abstract CameraEffectComposite GetCameraEffects(PlayerMovement movement, float deltaTime);

        protected virtual Vector3 GetCameraDirection(PlayerMovement movement, Vector2 direction)
        {
            Vector3 worldInputs = cam.transform.right * direction.x;
            float cameraDotProduct = Vector3.Dot(cam.transform.forward, -movement.Gravity.Value.normalized);
            
            worldInputs += cameraDotProduct switch
            {
                < -0.8f => cam.transform.up,
                > 0.8f => -cam.transform.up,
                _ => cam.transform.forward
            } * direction.y;
            
            return worldInputs;
        }
        
        protected virtual Vector3 GetWorldInputs(PlayerMovement movement)
        {
            return GetCameraDirection(movement, new Vector2(movement.InputDirection.x, movement.InputDirection.z));
        }
        
        protected virtual Vector3 GetGroundNormal(PlayerMovement movement)
        {
            return movement.GroundNormal;
        }
        
        public abstract MovementState State { get; }
    }
}