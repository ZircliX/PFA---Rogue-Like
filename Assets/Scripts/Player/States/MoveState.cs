using Unity.Cinemachine;
using UnityEngine;

namespace RogueLike.Player.States
{
    public abstract class MoveState : MovementStateBehavior
    {
        [SerializeField] private float maxSpeed;
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
        private float currentDeceleration;
        
        [Header("STEPSSSSSSSSSSSSSSSSSSSSSSSS")]
        [SerializeField] private float maxStepHeight;

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

        public override Vector3 GetVelocity(PlayerMovement movement, float deltaTime, ref float gravityScale)
        {
            Vector3 lastVelocity = movement.StateVelocity;
            Vector3 worldInputs = GetWorldInputs(movement);

            Vector3 projectionPlaneNormal = GetProjectionPlaneNormal(movement);
            Vector3 projectedInputs = worldInputs.ProjectOntoPlane(projectionPlaneNormal).normalized;
            Vector3 projectedLastDirection = direction.ProjectOntoPlane(projectionPlaneNormal).normalized;

            direction = Vector3.Lerp(projectedLastDirection, projectedInputs, directionControl * deltaTime);
            direction = projectedInputs;

            Vector3 planeVelocity = lastVelocity.ProjectOntoPlane(projectionPlaneNormal);
            Vector3 otherVelocity = lastVelocity - planeVelocity;

            Vector3 targetSpeed = direction * maxSpeed;

            float planeVelocitySqrMagnitude = planeVelocity.sqrMagnitude;

            //Debug . Log(planeVelocitySqrMagnitude);
            if (Mathf.Approximately(direction.sqrMagnitude, planeVelocitySqrMagnitude))
            {
                if (movement.IsGrounded && Vector3.Dot(otherVelocity, movement.Gravity) > 0)
                {
                    gravityScale = 0f;
                    return targetSpeed;
                }

                return targetSpeed + otherVelocity;
            }

            float modifier;
            if (planeVelocitySqrMagnitude < direction.sqrMagnitude) //Accelerate
            {
                //Debug.Log("Acceleatirheiern");
                currentAcceleration += deltaTime;
                currentDeceleration = 0;

                modifier = accelerationCurve.Evaluate(currentAcceleration / accelerationDuration) * acceleration;
            }
            else //Decelerate
            {
                //Debug.Log("Deceleuizhfuzeshfuehf");
                currentDeceleration += deltaTime;
                currentAcceleration = 0;

                modifier = accelerationCurve.Evaluate(currentDeceleration / decelerationDuration) * deceleration;
            }

            Vector3 finalVelocity = Vector3.Lerp(planeVelocity, targetSpeed, modifier * deltaTime);
            finalVelocity += otherVelocity;
            
            //Les escaliers mon pire enemi
            Vector3 up = -movement.Gravity.Value.normalized;
            Vector3 playerBasePosition = movement.Position - movement.CapsuleCollider.height * 0.5f * up;
            Vector3 stepPoint =  playerBasePosition + up * maxStepHeight + Vector3.ProjectOnPlane(finalVelocity, projectionPlaneNormal).normalized * movement.CapsuleCollider.radius;
            
            if (Physics.Raycast(stepPoint, -projectionPlaneNormal, out RaycastHit hit, maxStepHeight,
                    movement.GroundLayer))
            {
                float stepHeight = maxStepHeight - hit.distance;
                movement.rb.position += projectionPlaneNormal * stepHeight;
            }
            
            //Debug.DrawRay(stepPoint, -projectionPlaneNormal * maxStepHeight, Color.magenta);

            return finalVelocity;
        }
    }
}