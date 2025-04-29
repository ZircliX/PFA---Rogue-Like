using DeadLink.Cameras;
using DeadLink.Cameras.Data;
using UnityEngine;

namespace RogueLike.Player.States
{
    [CreateAssetMenu(menuName = "RogueLike/Movement/Crouch")]
    public class CrouchState : MoveState
    {
        [field: Header("Camera Effects")]
        [field: SerializeField] public CameraEffectData CameraEffectData { get; protected set; }
        
        [Header("Height")]
        [SerializeField] private float crouchCapsuleHeight = 0.5f;
        [SerializeField] private  float crouchHeadHeight = 0f;

        public override void Enter(PlayerMovement movement)
        {
            base.Enter(movement);
        }

        public override void Exit(PlayerMovement movement)
        {
            base.Exit(movement);
        }

        public override Vector3 GetVelocity(PlayerMovement movement, float deltaTime, ref float gravityScale)
        {
            gravityScale = 0;
            return base.GetVelocity(movement, deltaTime, ref gravityScale);
        }

        public override MovementState GetNextState(PlayerMovement movement)
        {
            if (!movement.IsGrounded)
            {
                return MovementState.Falling;
            }
            if (!movement.CrouchInput && !movement.IsTouchingCeiling)
            {
                return MovementState.Idle;
            }

            return State;
        }
        
        public override (float, float) GetHeight(PlayerMovement movement)
        {
            return (crouchCapsuleHeight, crouchHeadHeight);
        }

        public override CameraEffectComposite GetCameraEffects(PlayerMovement movement, float deltaTime)
        {
            return CameraEffectData.CameraEffectComposite;
        }

        public override MovementState State => MovementState.Crouching;
    }
}