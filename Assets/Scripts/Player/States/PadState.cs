using DeadLink.Cameras;
using DeadLink.Cameras.Data;
using DeadLink.Entities.Movement;
using DeadLink.Misc;
using UnityEngine;

namespace RogueLike.Player.States
{
    [CreateAssetMenu(menuName = "RogueLike/Movement/Pad")]
    public class PadState : MoveState
    {
        [Header("Pad")]
        [SerializeField] protected float padBaseForce;
        [SerializeField] protected float padDuration;
        [SerializeField] protected AnimationCurve padCurve;
        
        [field: SerializeField] public CameraEffectData CameraEffectData { get; protected set; }

        protected float currentPadTime;
        
        public override void Enter(EntityMovement movement)
        {
            base.Enter(movement);
            currentPadTime = 0;
            direction = movement.CurrentPad.PadDirection;
        }

        public override void Exit(EntityMovement movement)
        {
            base.Exit(movement);
            currentPadTime = 0;
            movement.ExitPad();
        }
        
        public override Vector3 GetVelocity(EntityMovement movement, float deltaTime, ref float gravityScale)
        {
            float normTime = currentPadTime / padDuration;
            float padModifier = padCurve.Evaluate(normTime);
            
            currentPadTime += deltaTime;
            if (currentPadTime >= padDuration)
                movement.SetMovementState(MovementState.Falling);

            Pad pad = movement.CurrentPad;
            Vector3 padDirection = pad.PadDirection.normalized;
            Vector3 finalVelocity = new Vector3(
                padDirection.x * pad.PadXForce,
                padDirection.y * pad.PadYForce,
                padDirection.z * pad.PadZForce) * padModifier * padBaseForce;

            return finalVelocity;
        }

        public override MovementState GetNextState(EntityMovement movement)
        {
            if (movement.CanWallRun())
            {
                return MovementState.WallRunning;
            }
            if (movement.CanDash())
            {
                return MovementState.Dashing;
            }

            return State;
        }

        public override (float, float) GetHeight(EntityMovement movement)
        {
            return (movement.BaseCapsuleHeight, movement.BaseHeadHeight);
        }

        public override CameraEffectComposite GetCameraEffects(EntityMovement movement, float deltaTime)
        {
            return CameraEffectData.CameraEffectComposite;
        }

        public override MovementState State => MovementState.Pad;
    }
}