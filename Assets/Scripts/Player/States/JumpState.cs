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
        
        public override void Initialize(PlayerMovement movement)
        {
        }

        public override void Dispose(PlayerMovement movement)
        {
        }

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

        public override Vector3 GetVelocity(PlayerMovement movement, float deltaTime)
        {
            float normTime = currentJumpTime / jumpDuration;
            float jumpModifier = jumpCurve.Evaluate(normTime);

            currentJumpTime += deltaTime;
            
            return -movement.Gravity.Value.normalized * (jumpModifier * jumpForce) + base.GetVelocity(movement, deltaTime);
        }

        public override MovementState State => MovementState.Jumping;
    }
}