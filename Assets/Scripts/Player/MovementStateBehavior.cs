using DeadLink.Cameras;
using DeadLink.Entities.Movement;
using UnityEngine;

namespace RogueLike.Player
{
    public abstract class MovementStateBehavior : ScriptableObject
    {
        protected Transform cam;

        public virtual void Initialize(EntityMovement movement)
        {
            cam = movement.CameraTransform;
        }
        public abstract void Dispose(EntityMovement movement);

        public abstract void Enter(EntityMovement movement);
        public abstract void Exit(EntityMovement movement);
        public abstract Vector3 GetVelocity(EntityMovement movement, float deltaTime, ref float gravityScale);

        public abstract MovementState GetNextState(EntityMovement movement);

        public abstract (float, float) GetHeight(EntityMovement movement);

        public abstract CameraEffectComposite GetCameraEffects(EntityMovement movement, float deltaTime);

        protected virtual Vector3 GetCameraDirection(EntityMovement movement, Vector2 direction)
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
        
        protected virtual Vector3 GetWorldInputs(EntityMovement movement)
        {
            return GetCameraDirection(movement, new Vector2(movement.InputDirection.x, movement.InputDirection.z));
        }
        
        protected virtual Vector3 GetGroundNormal(EntityMovement movement)
        {
            return movement.GroundNormal;
        }
        
        public abstract MovementState State { get; }
    }
}