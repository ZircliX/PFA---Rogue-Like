using UnityEngine;

namespace RogueLike.Player.States
{
    [CreateAssetMenu(menuName = "RogueLike/Movement/Slide")]
    public class SlideState : MoveState
    {
        [SerializeField] private float playerSlideHeight = 0.5f;
        [SerializeField] private float maxSlideTime = 1.5f;
        private float currentSlideTime;
        
        public override void Enter(PlayerMovement movement)
        {
            base.Enter(movement);
            currentSlideTime = 0;
            movement.transform.localScale = new Vector3(1, playerSlideHeight, 1);
            movement.rb.MovePosition(movement.rb.position - GetProjectionPlaneNormal(movement) * 0.5f);
        }

        public override void Exit(PlayerMovement movement)
        {
            base.Exit(movement);
            currentSlideTime = 0;
            movement.transform.localScale = new Vector3(1, 1, 1);
        }

        
        public override Vector3 GetVelocity(PlayerMovement movement, float deltaTime)
        {
            //Debug.Log((GetProjectionPlaneNormal(movement) - Vector3.up).sqrMagnitude);
            
            if ((GetProjectionPlaneNormal(movement) - Vector3.up).sqrMagnitude < 0.01f)
            {
                currentSlideTime += deltaTime;
            }

            if (currentSlideTime >= maxSlideTime)
            {
                movement.SetMovementState(MovementState.Running);
            }
            
            return base.GetVelocity(movement, deltaTime);
        }

        public override MovementState State => MovementState.Sliding;
    }
}