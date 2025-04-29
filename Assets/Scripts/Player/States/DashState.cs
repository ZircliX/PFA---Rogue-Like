using DeadLink.Cameras;
using DeadLink.Cameras.Data;
using UnityEngine;

namespace RogueLike.Player.States
{
    [CreateAssetMenu(menuName = "RogueLike/Movement/Dash")]
    public class DashState : MovementStateBehavior
    {
        [field: Header("Camera Effects")]
        [field: SerializeField] public CameraEffectData CameraEffectData { get; protected set; }
        
        [Header("Speed")]
        [SerializeField] private float dashSpeed;
        [SerializeField] private AnimationCurve accelerationCurve;
        [SerializeField] private float accelerationDuration;
        
        private float currentDashTime;
        private Vector3 direction;
        
        public override void Dispose(PlayerMovement movement)
        {
            
        }

        public override void Enter(PlayerMovement movement)
        {
            currentDashTime = 0;
            direction = GetCameraDirection(movement, Vector2.up);

            //Debug.Log("Enter Dash");
        }

        public override void Exit(PlayerMovement movement)
        {
            currentDashTime = 0;
            movement.DashCooldown();
            //Debug.Log("Exit Dash");
        }

        public override Vector3 GetVelocity(PlayerMovement movement, float deltaTime, ref float gravityScale)
        {
            Vector3 targetVelocity = Vector3.zero;
            Vector3 gravityNormal = movement.Gravity.Value.normalized;

            if (currentDashTime < accelerationDuration)
            {
                Vector3 projectOnPlane = Vector3.ProjectOnPlane(direction, gravityNormal);
                //Debug.DrawRay(movement.rb.position, projectOnPlane * 10, Color.green);
                Vector3 newVelocity =
                    projectOnPlane * accelerationCurve.Evaluate(currentDashTime / accelerationDuration) * dashSpeed;
                targetVelocity = newVelocity;
            }

            currentDashTime += deltaTime;
            return targetVelocity;
        }
        
        public override CameraEffectComposite GetCameraEffects(PlayerMovement movement, float deltaTime)
        {
            return CameraEffectData.CameraEffectComposite;
        }

        public override MovementState GetNextState(PlayerMovement movement)
        {
            if (currentDashTime >= accelerationDuration)
            {
                if (movement.IsGrounded)
                {
                    return MovementState.Walking;
                }
                if (movement.WantsToWallrun)
                {
                    return MovementState.Running;
                }
                if (movement.WantsToWallrun)
                {
                    return MovementState.WallRunning;
                }
                
                return MovementState.Falling;
            }

            return State;
        }

        public override (float, float) GetHeight(PlayerMovement movement)
        {
            return (movement.BaseCapsuleHeight, movement.BaseHeadHeight);
        }

        public override MovementState State => MovementState.Dashing;
    }
}