using UnityEngine;

namespace RogueLike.Player.States
{
    [CreateAssetMenu(menuName = "RogueLike/Movement/Slide")]
    public class SlideState : MovementStateBehavior
    {
        [Header("Speed")]
        [SerializeField] private float slideSpeed;
        [SerializeField] private float slideMinSpeed;
        [SerializeField] private AnimationCurve accelerationCurve;
        [SerializeField] private float accelerationDuration;
        [SerializeField] private float decelerationStrength;
        [SerializeField] private float decelerationThreshold;

        [Header("Slopes")]
        [SerializeField] private AnimationCurve slopeCurve;
        [SerializeField] private float slopeModifier;
        [SerializeField, Range(0, 1)] private float minSlopeAngle;
        
        [Header("Sliding")]
        [SerializeField] private float maxSlideTime = 1.5f;

        [Header("Height")]
        [SerializeField] private float headPositionOffset = 0.5f;
        [SerializeField] private float colliderHightOffset = 0.5f;
        
        private float currentSlideTime;
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
            currentSlideTime = 0;
            direction = GetCameraDirection(movement, Vector2.up);
            
            /*
            movement.Head.position -= Vector3.up * headPositionOffset;
            movement.CapsuleCollider.height -= colliderHightOffset;
            movement.CapsuleCollider.center -= Vector3.up * colliderHightOffset;
            */
        }

        public override void Exit(PlayerMovement movement)
        {
            currentSlideTime = 0;
            
            /*
            movement.Head.position += Vector3.up * headPositionOffset;
            movement.CapsuleCollider.height += colliderHightOffset;
            movement.CapsuleCollider.center += Vector3.up * colliderHightOffset;
            */
        }
        
        public override Vector3 GetVelocity(PlayerMovement movement, float deltaTime, ref float gravityScale)
        {
            Vector3 velocity = Vector3.zero;
            Vector3 projectionPlaneNormal = GetProjectionPlaneNormal(movement);
            
            //Acceleration
            if (currentSlideTime < accelerationDuration)
            {
                Vector3 projectOnPlane = Vector3.ProjectOnPlane(direction, projectionPlaneNormal);
                Vector3 newVelocity = projectOnPlane * accelerationCurve.Evaluate(currentSlideTime / accelerationDuration) * slideSpeed;
                velocity = newVelocity;
            }
            //Deceleration
            else
            {
                Vector3 currentVelocity = movement.StateVelocity;
                float dotProduct = Vector3.Dot(movement.Gravity.Value.normalized, Vector3.ProjectOnPlane(currentVelocity, projectionPlaneNormal).normalized);

                if (Mathf.Abs(dotProduct) <= minSlopeAngle)
                    velocity = Vector3.MoveTowards(currentVelocity, direction * slideMinSpeed, decelerationStrength * deltaTime);
                else
                    velocity = currentVelocity;
                
                float modifier = slopeCurve.Evaluate(dotProduct) * slopeModifier * deltaTime;
                velocity += direction.normalized * modifier;
            }

            currentSlideTime += deltaTime;
            return velocity;
        }

        public override MovementState GetNextState(PlayerMovement movement)
        {
            if (!movement.IsGrounded)
            {
                return MovementState.Falling;
            }
            if (movement.WantsToJump)
            {
                return MovementState.Jumping;
            }
            
            Vector3 projectionPlaneNormal = GetProjectionPlaneNormal(movement);
            Vector3 projectOnPlane = Vector3.ProjectOnPlane(movement.StateVelocity, projectionPlaneNormal);
            
            if (projectOnPlane.sqrMagnitude < slideMinSpeed * slideMinSpeed + decelerationThreshold)
            {
                return MovementState.Running;
            }

            return State;
        }

        public override MovementState State => MovementState.Sliding;
    }
}