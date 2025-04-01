using LTX.ChanneledProperties;
using UnityEngine;

namespace RogueLike.Player.States
{
    [CreateAssetMenu(menuName = "RogueLike/Movement/Walk")]
    public class WalkState : MovementStateBehavior
    {
        [SerializeField] private float maxSpeed;
        [SerializeField] private AnimationCurve accelerationCurve;
        [SerializeField] private AnimationCurve decelerationCurve;
        [SerializeField] private float accelerationTime;
        [SerializeField] private float decelerationTime;
        
        public override void Initialize(PlayerMovement movement)
        {
        }

        public override void Dispose(PlayerMovement movement)
        {
        }
        

        public override void Enter(PlayerMovement movement)
        {
            movement.CurrentVelocity.AddInfluence(this, Influence.Add, 1, 0);
        }

        public override void Exit(PlayerMovement movement)
        {
            movement.CurrentVelocity.RemoveInfluence(this);
        }

        public override void OnFixedUpdate(PlayerMovement movement)
        {
            Vector3 movementInputDirection = movement.InputDirection * maxSpeed;
            movement.CurrentVelocity.Write(this, movementInputDirection);
        }

        public override MovementState State => MovementState.Walking;
    }
}