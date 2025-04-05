using Unity.Cinemachine;
using UnityEngine;

namespace RogueLike.Player.States
{
    [CreateAssetMenu(menuName = "RogueLike/Movement/WallRun")]
    public class WallRunState : MovementStateBehavior
    {
        [Header("Speed")] 
        [SerializeField] private float wallrunSpeed;
        [SerializeField] private float minWallrunSpeed;
        
        [Header("Acceleration")]
        [SerializeField] private AnimationCurve accelerationCurve;
        [SerializeField] private float accelerationDuration;
        [SerializeField] private float acceleration;
        private float currentAcceleration;
        
        [Header("Deceleration")]
        [SerializeField] private AnimationCurve decelerationCurve;
        [SerializeField] private float decelerationDuration;
        [SerializeField] private float deceleration;
        [SerializeField] private float decelerationThreshold;
        private float currentDeceleration;

        [Header("Wall")] 
        [SerializeField] private AnimationCurve slopeCurve;
        [SerializeField] private float slopeModifier;
        [SerializeField, Range(0, 1)] private float minSlopeAngle;
        
        [Header("Wall Running")]
        [SerializeField] private float maxWallrunTime = 5f;
        
        private float currentWallrunTime;
        private Vector3 velocityDirection;
        private Vector3 camDirection;
        private Camera cam;
        
        public override void Initialize(PlayerMovement movement)
        {
            cam = Camera.main;
        }

        public override void Dispose(PlayerMovement movement)
        {
        }

        public override void Enter(PlayerMovement movement)
        {
            velocityDirection = movement.StateVelocity.sqrMagnitude > 0.1f ? movement.StateVelocity.normalized : Vector3.zero;
            camDirection = GetCameraDirection(movement, Vector2.up);
            
            currentWallrunTime = 0;
            currentAcceleration = 0;
            currentDeceleration = 0;
        }

        public override void Exit(PlayerMovement movement)
        {
            currentWallrunTime = 0;
            currentAcceleration = 0;
            currentDeceleration = 0;
        }

        public override Vector3 GetVelocity(PlayerMovement movement, float deltaTime)
        {
            Vector3 lastVelocity = movement.StateVelocity;
            Vector3 worldInput = GetWorldInputs(movement);
            Vector3 wallNormal = movement.WallNormal;
            
            Vector3 projectedInputs = worldInput.ProjectOntoPlane(wallNormal).normalized;
            Vector3 projectedLastDirection = velocityDirection.ProjectOntoPlane(wallNormal);

            //Needs to lerp direction
            
            Vector3 wallVelocity = lastVelocity.ProjectOntoPlane(wallNormal);
            Vector3 otherVelocity = lastVelocity - wallVelocity;
            
            Vector3 targetSpeed = projectedLastDirection * wallrunSpeed;

            float wallVelocitySqrMagnitude = wallVelocity.sqrMagnitude;

            if (Mathf.Approximately(velocityDirection.sqrMagnitude, wallVelocitySqrMagnitude))
            {
                if (movement.IsWalled && Vector3.Dot(otherVelocity, wallNormal) > 0)
                {
                    return targetSpeed;
                }
            }
            
            float modifier;
            //Accelerate
            if (wallVelocitySqrMagnitude < velocityDirection.sqrMagnitude)
            {
                currentAcceleration += deltaTime;
                currentDeceleration = 0;
                modifier = accelerationCurve.Evaluate(currentAcceleration / accelerationDuration) * acceleration;
            }
            //Decelerate
            else
            {
                currentDeceleration += deltaTime;
                currentAcceleration = 0;
                modifier = decelerationCurve.Evaluate(currentDeceleration / decelerationDuration) * deceleration;
            }

            Vector3 finalVelocity = Vector3.Lerp(wallVelocity, targetSpeed, modifier * deltaTime);

            if (movement.IsWalled)
            {
                return finalVelocity;
            }

            finalVelocity += otherVelocity;

            return finalVelocity;
        }

        public override MovementState GetNextState(PlayerMovement movement)
        {
            if (movement.WantsToJump)
            {
                return MovementState.Jumping;
            }
            if (!movement.WantsToWallrun)
            {
                return MovementState.Falling;
            }

            Vector3 wallNormal = movement.WallNormal;
            Vector3 projectOnPlane = Vector3.ProjectOnPlane(movement.StateVelocity, wallNormal);

            if (projectOnPlane.sqrMagnitude < minWallrunSpeed * minWallrunSpeed + decelerationThreshold)
            {
                return MovementState.Falling;
            }
            
            return State;
        }

        public override MovementState State => MovementState.WallRunning;
    }
}