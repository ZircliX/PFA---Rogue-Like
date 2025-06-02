using DeadLink.Cameras;
using DeadLink.Entities.Movement;
using DeadLink.Misc;
using UnityEngine;

namespace RogueLike.Player.States
{
    [CreateAssetMenu(menuName = "RogueLike/Movement/Pad")]
    public class PadState : MoveState
    {
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
            Pad pad = movement.CurrentPad;
            
            currentPadTime += deltaTime;
            if (currentPadTime >= pad.PadDuration)
                movement.SetMovementState(MovementState.Falling);

            Vector3 baseVelocity = base.GetVelocity(movement, deltaTime, ref gravityScale);
            
            Vector3 finalVelocity = CalculateVelocity(pad, currentPadTime) + baseVelocity;
            
            return finalVelocity;
        }

        public static Vector3 CalculateVelocity(Pad pad, float currentTime)
        {
            float normTime = currentTime / pad.PadDuration;
            float padModifier = pad.PadCurve.Evaluate(normTime);
            
            Vector3 finalVelocity = pad.PadDirection * padModifier;
            
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
            return movement.CurrentPad.CameraEffectData.CameraEffectComposite;
        }

        public override MovementState State => MovementState.Pad;
    }
}