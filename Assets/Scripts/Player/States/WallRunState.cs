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
        [SerializeField] private float directionControl;
        
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
        [SerializeField] private float wallPull;
        
        private Vector3 direction;
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
            direction = movement.StateVelocity.sqrMagnitude > 0.1f ? movement.StateVelocity.normalized : Vector3.zero;
            
            currentAcceleration = 0;
            currentDeceleration = 0;
        }

        public override void Exit(PlayerMovement movement)
        {
            currentAcceleration = 0;
            currentDeceleration = 0;
        }

        public override Vector3 GetVelocity(PlayerMovement movement, float deltaTime)
        {
            Vector3 lastVelocity = movement.StateVelocity;
            
            //Calculate wallrun direction
            Vector3 wallNormal = movement.WallNormal;
            Vector3 alongWallDirection = Vector3.Cross(wallNormal.normalized, movement.Gravity.Value.normalized);
            
            float angle = Vector3.Dot(alongWallDirection, lastVelocity);
            if (angle < 0)
            {
                //invert direction
                alongWallDirection = -alongWallDirection;
            }
            
            //For inputs based movement
            //Vector3 worldInput = GetWorldInputs(movement);
            //Vector3 projectedInputsDirection = worldInput.ProjectOntoPlane(wallNormal).normalized;
            
            //Vector3 projectedLastDirection = direction.ProjectOntoPlane(wallNormal);
            //projectedLastDirection = Vector3.Lerp(projectedLastDirection, projectedInputsDirection, directionControl * deltaTime);
            //Vector3 targetSpeed = projectedLastDirection * wallrunSpeed;
            
            //Velocities calculation
            Vector3 wallPullForce = wallPull * -wallNormal;
            Vector3 wallVelocity = lastVelocity.ProjectOntoPlane(wallNormal) + wallPullForce;
            Vector3 targetSpeed = alongWallDirection * wallrunSpeed;

            //Sqr Magnitudes
            float wallVelocitySqrMagnitude = wallVelocity.sqrMagnitude;
            float directionSqrMagnitude = direction.sqrMagnitude;

            //Wanted speed
            if (Mathf.Approximately(directionSqrMagnitude, wallVelocitySqrMagnitude))
            {
                targetSpeed = targetSpeed.ProjectOntoPlane(movement.Gravity.Value.normalized);
                return targetSpeed;
            }
            //Accelerate
            float modifier;
            if (wallVelocitySqrMagnitude < directionSqrMagnitude)
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
            finalVelocity = finalVelocity.ProjectOntoPlane(movement.Gravity.Value.normalized);

            return finalVelocity;
        }

        public override MovementState GetNextState(PlayerMovement movement)
        {
            if (movement.WantsToJump)
            {
                movement.ExitWallrun();
                return MovementState.Jumping;
            }
            if (!movement.WantsToWallrun)
            {
                movement.ExitWallrun();
                return MovementState.Falling;
            }

            Vector3 wallNormal = movement.WallNormal;
            Vector3 projectOnPlane = Vector3.ProjectOnPlane(movement.StateVelocity, wallNormal);

            if (projectOnPlane.sqrMagnitude < minWallrunSpeed * minWallrunSpeed + decelerationThreshold)
            {
                movement.ExitWallrun();
                return MovementState.Falling;
            }
            
            return State;
        }

        public override MovementState State => MovementState.WallRunning;
    }
}