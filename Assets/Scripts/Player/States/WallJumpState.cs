using DeadLink.Cameras;
using UnityEngine;

namespace RogueLike.Player.States
{
    [CreateAssetMenu(menuName = "RogueLike/Movement/WallJump")]
    public class WallJumpState : JumpState
    {
        [Header("Wall Jump")]
        [SerializeField] private float diagonalJumpForce;

        public override Vector3 GetVelocity(PlayerMovement movement, float deltaTime, ref float gravityScale)
        {
            float normTime = currentJumpTime / jumpDuration;

            float jumpModifier = jumpCurve.Evaluate(normTime);

            Vector3 baseVelocity = base.GetVelocity(movement, deltaTime, ref gravityScale);
            Vector3 gravityNormal = GetGroundNormal(movement);

            if (currentJumpTime <= 0)
            {
                //Debug.Log($"before : {baseVelocity} ");
                baseVelocity = Vector3.ProjectOnPlane(baseVelocity, gravityNormal);
                //Debug.Log($"after : {baseVelocity} ");
            }

            currentJumpTime += deltaTime;
            if (currentJumpTime >= jumpDuration)
                movement.SetMovementState(MovementState.Falling);

            Vector3 finalVelocity = gravityNormal * (jumpModifier * jumpForce) + movement.WallNormal * (jumpModifier * diagonalJumpForce) + baseVelocity;

            //Debug.Log($"{count++} | {baseVelocity.y} => {finalVelocity.magnitude}");

            return finalVelocity;
        }
        
        public override CameraEffectComposite GetCameraEffects(PlayerMovement movement, float deltaTime)
        {
            Vector3 cross = Vector3.Cross(movement.WallNormal, movement.Gravity.Value.normalized);
            float dot = Vector3.Dot(cross, movement.CurrentVelocity.Value.normalized);

            CameraEffectComposite comp = CameraEffectData.CameraEffectComposite;
            CameraEffectComposite cameraEffectComposite = new CameraEffectComposite(
                dot > 0 ? comp.Dutch : -comp.Dutch, 
                comp.FovScale, 
                comp.Speed);
            
            return cameraEffectComposite;
        }
        
        public override MovementState State => MovementState.WallJumping;
    }
}