using Unity.Cinemachine;
using UnityEngine;

namespace RogueLike.Player.States
{
    public abstract class MoveState : MovementStateBehavior
    {
        [SerializeField] private float maxSpeed;
        [SerializeField] private float directionControl;
        [SerializeField] private AnimationCurve accelerationCurve;
        [SerializeField] private AnimationCurve decelerationCurve;
        [SerializeField] private float accelerationDuration;
        [SerializeField] private float decelerationDuration;
        private float currentAcceleration;
        private float currentDeceleration;

        private Vector3 startSpeed;
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
            startSpeed = movement.StateVelocity.ProjectOntoPlane(movement.GroundNormal);
            direction = startSpeed.sqrMagnitude > 0.1f ? startSpeed.normalized : Vector3.zero;
            
            currentAcceleration = 0;
            currentDeceleration = 0;
        }

        public override void Exit(PlayerMovement movement)
        {
            currentAcceleration = 0;
            currentDeceleration = 0;
        }

        protected virtual Vector3 GetProjectionPlaneNormal(PlayerMovement movement)
        {
            return movement.GroundNormal;
        }

        protected virtual float GetCameraDotProduct(PlayerMovement movement)
        {
            return Vector3.Dot(cam.transform.forward, -movement.Gravity.Value.normalized); 
        }

        public override Vector3 GetVelocity(PlayerMovement movement, float deltaTime)
        {
            Vector3 worldInputs = cam.transform.right * movement.InputDirection.x;

            worldInputs += GetCameraDotProduct(movement) switch
            {
                < -0.8f => cam.transform.up,
                > 0.8f => -cam.transform.up,
                _ => cam.transform.forward
            } * movement.InputDirection.z;

            Vector3 project = worldInputs.ProjectOntoPlane(GetProjectionPlaneNormal(movement)).normalized;
            direction = Vector3.Lerp(direction, project, directionControl * deltaTime);
            
            Vector3 targetSpeed = direction * maxSpeed;

            float speed = maxSpeed * maxSpeed;
            float startSpeedSqrMagnitude = startSpeed.sqrMagnitude;

            if (Mathf.Approximately(speed, startSpeedSqrMagnitude))
            {
                //Debug.Log("max aqiscjqiscjqi");
                return targetSpeed;
            }
            
            if (startSpeedSqrMagnitude < speed) //Accelerate
            {
                //Debug.Log("Acceleatirheiern");
                currentAcceleration += deltaTime;
                currentDeceleration = 0;

                //Debug.Log(currentAcceleration / accelerationDuration);
                float modifier = accelerationCurve.Evaluate(currentAcceleration / accelerationDuration);
                return Vector3.Lerp(startSpeed, targetSpeed, modifier);
            }
            else //Decelerate
            {
                //Debug.Log("Deceleuizhfuzeshfuehf");
                currentDeceleration += deltaTime;
                currentAcceleration = 0;
                
                float modifier = accelerationCurve.Evaluate(currentDeceleration / decelerationDuration);
                return Vector3.Lerp(startSpeed, targetSpeed, modifier);
            }
        }
    }
}