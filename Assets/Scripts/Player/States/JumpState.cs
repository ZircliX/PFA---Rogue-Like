using UnityEngine;

namespace RogueLike.Player.States
{
    [CreateAssetMenu(menuName = "RogueLike/Movement/Jump")]
    public class JumpState : FallState
    {
        [SerializeField] private float jumpForce;
        [SerializeField] private float jumpDuration;
        [SerializeField] private AnimationCurve jumpCurve;
        private float currentJumpTime;
        
        public override void Enter(PlayerMovement movement)
        {
            base.Enter(movement);
            
            //Debug.Log("JUMPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPP");
            currentJumpTime = 0;
        }

        public override void Exit(PlayerMovement movement)
        {
            base.Exit(movement);
            
            currentJumpTime = 0;
        }

        protected override Vector3 GetProjectionPlaneNormal(PlayerMovement movement)
        {
            return -movement.Gravity.Value.normalized;
        }

        public override Vector3 GetVelocity(PlayerMovement movement, float deltaTime)
        {
            float normTime = currentJumpTime / jumpDuration;
            
            float jumpModifier = jumpCurve.Evaluate(normTime);

            currentJumpTime += deltaTime;

            Vector3 baseVelocity = base.GetVelocity(movement, deltaTime);
            Vector3 gravityNormal = GetProjectionPlaneNormal(movement);
            
            if (normTime >= 1)
            {
                movement.SetVelocity(gravityNormal * (jumpModifier * jumpForce));
                movement.SetMovementState(MovementState.Falling);
                return baseVelocity;
            }
            
            //Debug.Log(gravityNormal * (jumpModifier * jumpForce) + baseVelocity);
            return gravityNormal * (jumpModifier * jumpForce) + baseVelocity ;
        }

        public override MovementState State => MovementState.Jumping;
    }
}