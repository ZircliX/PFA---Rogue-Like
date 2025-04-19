using DeadLink.Cameras;
using DeadLink.Cameras.Data;
using Unity.Cinemachine;
using UnityEngine;

namespace RogueLike.Player.States
{
    [CreateAssetMenu(menuName = "RogueLike/Movement/WallRun")]
    public class WallRunState : MovementStateBehavior
    {
        [field: Header("Camera Effects")]
        [field: SerializeField] public CameraEffectData CameraEffectData { get; protected set; }
        
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
        private Vector3 wallNormal;
        private Camera cam;

        public override void Initialize(PlayerMovement movement)
        {
            cam = movement.Camera;
        }

        public override void Dispose(PlayerMovement movement)
        {
        }

        public override void Enter(PlayerMovement movement)
        {
            direction = movement.StateVelocity.sqrMagnitude > 0.1f ? movement.StateVelocity.normalized : Vector3.zero;
            wallNormal = movement.WallNormal;
            
            currentAcceleration = 0;
            currentDeceleration = 0;
        }

        public override void Exit(PlayerMovement movement)
        {
            currentAcceleration = 0;
            currentDeceleration = 0;
        }

        public override CameraEffectComposite GetCameraEffects(PlayerMovement movement, float deltaTime)
        {
            Vector3 cross = Vector3.Cross(wallNormal, movement.Gravity.Value.normalized);
            float dot = Vector3.Dot(cross, movement.CurrentVelocity.Value.normalized);

            CameraEffectComposite comp = CameraEffectData.CameraEffectComposite;
            CameraEffectComposite cameraEffectComposite = new CameraEffectComposite(
                dot > 0 ? comp.Dutch : -comp.Dutch, 
                comp.FovScale, 
                comp.Speed);
            
            return cameraEffectComposite;
        }

        public override Vector3 GetVelocity(PlayerMovement movement, float deltaTime, ref float gravityScale)
        {
            Vector3 lastVelocity = movement.StateVelocity;
            
            //Calculate wallrun direction
            Vector3 alongWallDirection = Vector3.Cross(wallNormal, movement.Gravity).normalized;
            
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
                if (targetSpeed.y <= 0)
                {
                    targetSpeed = targetSpeed.ProjectOntoPlane(movement.Gravity.Value.normalized);
                }
                
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
            if (finalVelocity.y <= 0)
            {
                finalVelocity = finalVelocity.ProjectOntoPlane(movement.Gravity.Value.normalized);
            }

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

        public override (float, float) GetHeight(PlayerMovement movement)
        {
            return (movement.BaseCapsuleHeight, movement.BaseHeadHeight);
        }

        public override MovementState State => MovementState.WallRunning;
    }
}