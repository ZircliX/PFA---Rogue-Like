using KBCore.Refs;
using LTX.ChanneledProperties;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RogueLike.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [System.Serializable]
        private class MovementStateStatus
        {
            public MovementStateBehavior behavior;
            public bool isActive;
        }
        
        public Vector3 InputDirection { get; private set; }
        public MovementState CurrentState { get; private set; }
        public InfluencedProperty<Velocity> CurrentVelocity { get; private set; }
        public PrioritisedProperty<Vector3> Gravity { get; private set; }

        [SerializeField] private float groundCheckDistance;
        [SerializeField] private float groundCheckMaxAngle;

        [SerializeField] private int coyoteTime = 10;
        [SerializeField, Self] private Rigidbody rb;

        [SerializeField] private MovementStateStatus[] movementStates;
        
        private bool runInput;
        private int jumpInput;
        public bool WantsToJump => jumpInput > 0;
        private int slideInput;
        private bool WantsToSlide => slideInput > 0;
        private bool crouchInput;
        public bool IsGrounded { get; private set; }

        private void OnValidate()
        {
            this.ValidateRefs();
        }

        private void Awake()
        {
            CurrentVelocity = new InfluencedProperty<Velocity>(Vector3.zero);
            Gravity = new PrioritisedProperty<Vector3>(Vector3.down * 9.81f);
            
            for (int i = 0; i < movementStates.Length; i++)
            {
                MovementStateStatus state = movementStates[i];
                state.behavior.Initialize(this);
            }
        }
        private void Start()
        {
            SetMovementState(MovementState.Walking);
        }
        
        private void OnDestroy()
        {
            for (int i = 0; i < movementStates.Length; i++)
            {
                MovementStateStatus state = movementStates[i];
                state.behavior.Dispose(this);
            }
        }
        
        private void Update()
        {
            if (IsGrounded)
            {
                if (runInput)
                {
                    SetMovementState(MovementState.Running);
                }
                else
                {
                    SetMovementState(MovementState.Walking);
                }
            }
            else
            {
                SetMovementState(MovementState.Air);
            }
        }

        private void FixedUpdate()
        {
            HandleGroundDetection();
            
            if (jumpInput > 0)
            {
                jumpInput --;
            }
            if (slideInput > 0)
            {
                slideInput --;
            }
            
            for (int i = 0; i < movementStates.Length; i++)
            {
                MovementStateStatus state = movementStates[i];
                if (state.isActive)
                {
                    state.behavior.OnFixedUpdate(this);
                }
            }

            MovePlayer();
        }

        private void MovePlayer()
        {
            Vector3 finalVelocity = CurrentVelocity.Value;
            
            rb.MovePosition(rb.position + finalVelocity * Time.deltaTime);
        }
        
        private void HandleGroundDetection()
        {
            RaycastHit[] results = rb.SweepTestAll(Gravity.Value.normalized, groundCheckDistance, QueryTriggerInteraction.Ignore);
            for (int i = 0; i < results.Length; i++)
            {
                RaycastHit res = results[i];
                if (Vector3.Angle(res.normal, -Gravity.Value.normalized) < groundCheckMaxAngle)
                {
                    IsGrounded = true;
                    return;
                }
            }

            IsGrounded = false;
        }

        private void SetMovementState(MovementState state)
        {
            for (int i = 0; i < movementStates.Length; i++)
            {
                MovementStateStatus movementStateBehavior = movementStates[i];
                if (movementStateBehavior.behavior.State == state)
                {
                    if (!movementStateBehavior.isActive)
                    {
                        movementStateBehavior.isActive = true;
                        movementStateBehavior.behavior.Enter(this);
                    }
                }
                else
                {
                    if (movementStateBehavior.isActive)
                    {
                        movementStateBehavior.isActive = false;
                        movementStateBehavior.behavior.Exit(this);
                    }
                }
            }
        }
        
        
        #region Inputs
        
        public void ReadInputMove(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            InputDirection = new Vector3(input.x, 0, input.y);
        }

        public void ReadInputJump(InputAction.CallbackContext context)
        {
            //Buffer
            if (context.performed)
            {
                jumpInput = coyoteTime;
            }
            else if (context.canceled)
            {
                jumpInput = 0;
            }
        }
        
        public void ReadInputRun(InputAction.CallbackContext context)
        {
            runInput = context.performed;
        }
        
        public void ReadInputCrouch(InputAction.CallbackContext context)
        {
            crouchInput = context.performed;
        }
        
        public void ReadInputSlide(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                slideInput = coyoteTime;
            }
            else if (context.canceled)
            {
                slideInput = 0;
            }
        }
        
        #endregion

    }
}