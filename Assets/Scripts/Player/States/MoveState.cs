using RogueLike.Controllers;
using Unity.Cinemachine;
using UnityEngine;

namespace RogueLike.Player.States
{
    public abstract class MoveState : MovementStateBehavior
    {
        [SerializeField] private float maxSpeed;
        [SerializeField] private AnimationCurve accelerationCurve;
        [SerializeField] private AnimationCurve decelerationCurve;
        [SerializeField] private float accelerationDuration;
        [SerializeField] private float decelerationDuration;
        private float currentAcceleration;
        private float currentDeceleration;

        private Vector3 startSpeed;
        private Camera cam;
        public override void Initialize(PlayerMovement movement)
        {
        }

        public override void Dispose(PlayerMovement movement)
        {
        }
        

        public override void Enter(PlayerMovement movement)
        {
            cam = Camera.main;
            startSpeed = movement.StateVelocity.ProjectOntoPlane(movement.GroundNormal);
            
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
            Vector3 worldInputs = cam.transform.right * movement.InputDirection.x +
                                  cam.transform.forward * movement.InputDirection.z;
            
            Vector3 project = worldInputs.ProjectOntoPlane(movement.GroundNormal).normalized;
            
            Vector3 targetSpeed = project * maxSpeed;

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