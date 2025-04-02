using Unity.Cinemachine;
using UnityEngine;

namespace RogueLike.Player.States
{
    [CreateAssetMenu(menuName = "RogueLike/Movement/Jump")]
    public class JumpState : FallState
    {
        [Header("Jump")]
        [SerializeField] private float jumpForce;
        [SerializeField] private float jumpDuration;
        [SerializeField] private AnimationCurve jumpCurve;
        private float currentJumpTime;


        private int count;
        public override void Enter(PlayerMovement movement)
        {
            base.Enter(movement);

            count = 0;
            currentJumpTime = 0;
        }

        public override void Exit(PlayerMovement movement)
        {
            base.Exit(movement);
            currentJumpTime = 0;
        }

        public override Vector3 GetVelocity(PlayerMovement movement, float deltaTime)
        {
            float normTime = currentJumpTime / jumpDuration;

            float jumpModifier = jumpCurve.Evaluate(normTime);

            Vector3 baseVelocity = base.GetVelocity(movement, deltaTime);
            Vector3 gravityNormal = GetProjectionPlaneNormal(movement);

            if (currentJumpTime <= 0)
            {
                //Debug.Log($"before : {baseVelocity} ");
                baseVelocity = Vector3.ProjectOnPlane(baseVelocity, gravityNormal);
                //Debug.Log($"after : {baseVelocity} ");
            }

            currentJumpTime += deltaTime;
            if (currentJumpTime >= jumpDuration)
                movement.SetMovementState(MovementState.Falling);

            Vector3 finalVelocity = movement.ApplyGravity(gravityNormal * (jumpModifier * jumpForce) + baseVelocity);

            // Debug.Log($"{count++} | {baseVelocity.y} => {finalVelocity.y}");

            return finalVelocity;
        }

        public override MovementState State => MovementState.Jumping;
    }
}