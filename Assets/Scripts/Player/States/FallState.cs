using DeadLink.Cameras;
using DeadLink.Cameras.Data;
using UnityEngine;

namespace RogueLike.Player.States
{
    [CreateAssetMenu(menuName = "RogueLike/Movement/Fall")]
    public class FallState : MoveState
    {
        [field: Header("Camera Effects")]
        [field: SerializeField] public FallCameraEffectData CameraEffectData { get; protected set; }
        private float fovModifier;

        public override void Enter(PlayerMovement movement)
        {
            fovModifier = 0;
        }

        public override void Exit(PlayerMovement movement)
        {
            fovModifier = 0;
        }

        public override MovementState GetNextState(PlayerMovement movement)
        {
            if (movement.IsGrounded)
            {
                MovementState nextState = MovementState.Idle;

                if (movement.CrouchInput)
                    nextState = MovementState.Crouching;
                else if (movement.InputDirection.sqrMagnitude > PlayerMovement.MIN_THRESHOLD)
                {
                    nextState = movement.WalkInput ? MovementState.Walking : MovementState.Running;
                }

                return nextState;
            }
            if (movement.WantsToWallrun)
            {
                return MovementState.WallRunning;
            }

            if (movement.WantsToDash)
            {
                return MovementState.Dashing;
            }
            
            return State;
        }

        public override (float, float) GetHeight(PlayerMovement movement)
        {
            return (movement.BaseCapsuleHeight, movement.BaseHeadHeight);
        }

        public override CameraEffectComposite GetCameraEffects(PlayerMovement movement, float deltaTime)
        {
            CameraEffectComposite composite = CameraEffectData.CameraEffectComposite;

            fovModifier = Mathf.Min(
                fovModifier + deltaTime * CameraEffectData.FovMultiplier,
                CameraEffectData.MaxFov - composite.FovScale
            );

            composite.FovScale += fovModifier;
            return composite;
        }

        protected override Vector3 GetProjectionPlaneNormal(PlayerMovement movement)
        {
            return -movement.Gravity.Value.normalized;
        }
        
        public override MovementState State => MovementState.Falling;
    }
}