using KBCore.Refs;
using LTX.ChanneledProperties;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RogueLike.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public bool IsGrounded { get; private set; }
        public Vector3 InputDirection { get; private set; }
        public Vector3 GroundNormal { get; private set; }
        public Vector3 StateVelocity => CurrentVelocity.GetValue(stateChannelKey);
        public MovementState CurrentState { get; private set; }
        public InfluencedProperty<Vector3> CurrentVelocity { get; private set; }
        public PrioritisedProperty<Vector3> Gravity { get; private set; }
        
        [Header("Movement States")]
        [SerializeField] private MovementStateBehavior[] movementStates;

        [Header("Ground")]
        [SerializeField] private float groundCheckDistance = 0.1f;
        [SerializeField] private float groundCheckMaxAngle = 50;
        [field: SerializeField] public LayerMask GroundLayer { get; private set; }

        [Header("Gravity")]
        [SerializeField] private float gravityScale = 1;
        
        [Header("Coyote")]
        [SerializeField] private int coyoteTime = 10;

        
        [field: SerializeField, Self, Space] public Rigidbody rb { get; private set; }
        [SerializeField, Child] private CapsuleCollider cc;

        private bool runInput;
        private bool crouchInput;

        private int jumpInput;
        private bool WantsToJump => jumpInput > 0;
        private int slideInput;
        private bool WantsToSlide => slideInput > 0;

        private ChannelKey stateChannelKey;

        private const float MinMoveInputThresholdSqr = 0.01f;

        private void OnValidate() => this.ValidateRefs();

        private void Awake()
        {
            CurrentVelocity = new InfluencedProperty<Vector3>(Vector3.zero);
            stateChannelKey = ChannelKey.GetUniqueChannelKey();
            CurrentVelocity.AddInfluence(stateChannelKey, Influence.Add, 1, 0);

            Gravity = new PrioritisedProperty<Vector3>(Vector3.down * 9.81f);

            for (int i = 0; i < movementStates.Length; i++)
            {
                MovementStateBehavior state = movementStates[i];
                state.Initialize(this);
            }
        }

        private void Start()
        {
            SetMovementState(MovementState.Walking);
        }

        private void OnDestroy()
        {
            CurrentVelocity.RemoveInfluence(stateChannelKey);

            for (int i = 0; i < movementStates.Length; i++)
            {
                MovementStateBehavior state = movementStates[i];
                state.Dispose(this);
            }
        }

        private void FixedUpdate()
        {
            HandleGroundDetection();
            HandleStateChange();

            //Set Buffers
            if (jumpInput > 0)
                jumpInput --;
            if (slideInput > 0)
                slideInput --;

            float deltaTime = Time.fixedDeltaTime;
            for (int i = 0; i < movementStates.Length; i++)
            {
                MovementStateBehavior state = movementStates[i];
                if (state.State == CurrentState)
                {
                    Vector3 result = state.GetVelocity(this, deltaTime);
                    CurrentVelocity.Write(stateChannelKey, result);
                }
            }

            MovePlayer();
        }

        private void MovePlayer()
        {
            Vector3 velocity = CurrentVelocity.Value;
            rb.linearVelocity = velocity;
        }


        public Vector3 ApplyGravity(Vector3 baseVelocity)
        {
            return ApplyGravity(baseVelocity, Time.deltaTime);
        }

        public Vector3 ApplyGravity(Vector3 baseVelocity, float deltaTime)
        {
            return baseVelocity + Gravity.Value * (gravityScale * deltaTime);
        }

        private void HandleStateChange()
        {
            switch (CurrentState)
            {
                case MovementState.Falling:
                    if (IsGrounded)
                    {
                        MovementState nextState = MovementState.Idle;

                        if (crouchInput)
                            nextState = MovementState.Crouching;
                        else if (InputDirection.sqrMagnitude > MinMoveInputThresholdSqr)
                        {
                            nextState = runInput ? MovementState.Running : MovementState.Walking;
                        }

                        SetMovementState(nextState);
                    }

                    break;

                case MovementState.Jumping:
                    if (CurrentVelocity.Value.y < -0.2f)
                    {
                        SetMovementState(MovementState.Falling);
                    }
                    break;

                case MovementState.Idle:
                    if (!IsGrounded)
                    {
                        SetMovementState(MovementState.Falling);
                        break;
                    }
                    if (WantsToJump)
                    {
                        SetMovementState(MovementState.Jumping);
                        jumpInput = 0;
                        break;
                    }
                    if (crouchInput)
                    {
                        SetMovementState(MovementState.Crouching);
                        break;
                    }
                    if (InputDirection.sqrMagnitude > MinMoveInputThresholdSqr)
                    {
                        SetMovementState(MovementState.Walking);
                        break;
                    }

                    break;

                case MovementState.Crouching:
                    if (!IsGrounded)
                    {
                        SetMovementState(MovementState.Falling);
                        break;
                    }
                    if (!crouchInput)
                    {
                        SetMovementState(MovementState.Idle);
                        break;
                    }

                    break;

                case MovementState.Walking:
                    if (!IsGrounded)
                    {
                        SetMovementState(MovementState.Falling);
                        break;
                    }
                    if (WantsToJump)
                    {
                        SetMovementState(MovementState.Jumping);
                        jumpInput = 0;
                        break;
                    }
                    if (crouchInput)
                    {
                        SetMovementState(MovementState.Crouching);
                        break;
                    }
                    if (runInput)
                    {
                        SetMovementState(MovementState.Running);
                        break;
                    }
                    if (InputDirection.sqrMagnitude < MinMoveInputThresholdSqr)
                    {
                        SetMovementState(MovementState.Idle);
                        break;
                    }

                    break;

                case MovementState.Running:
                    if (!IsGrounded)
                    {
                        SetMovementState(MovementState.Falling);
                        break;
                    }
                    if (WantsToJump)
                    {
                        SetMovementState(MovementState.Jumping);
                        jumpInput = 0;
                        break;
                    }
                    if (WantsToSlide)
                    {
                        SetMovementState(MovementState.Sliding);
                        slideInput = 0;
                        break;
                    }
                    if (InputDirection.sqrMagnitude < MinMoveInputThresholdSqr)
                    {
                        SetMovementState(MovementState.Idle);
                        break;
                    }
                    if (!runInput)
                    {
                        SetMovementState(MovementState.Walking);
                        break;
                    }

                    break;

                case MovementState.Sliding:
                    if (!IsGrounded)
                    {
                        SetMovementState(MovementState.Falling);
                        break;
                    }
                    if (WantsToJump)
                    {
                        SetMovementState(MovementState.Jumping);
                        jumpInput = 0;
                        break;
                    }

                    // *** IMPORTANT: No normal slide end condition HERE ***
                    // Per your clarification, the logic script dedicated to sliding
                    // handles the transition back to Running when the slide naturally ends.
                    // This switch only handles external interruptions like Jump or Falling.
                    break;

            }
        }

        private void HandleGroundDetection()
        {
            if (CurrentState == MovementState.Jumping && StateVelocity.y >= 0)
            {
                GroundNormal = Vector3.up;
                IsGrounded = false;
                return;
            }

            Vector3 gravityNormalized = Gravity.Value.normalized;

            bool result = Physics.SphereCast(rb.position, cc.radius, gravityNormalized, out RaycastHit hit, groundCheckDistance + cc.height * 0.5f - cc.radius, GroundLayer);

            //Debug.DrawRay(rb.position, gravityNormalized * (groundCheckDistance + cc.height * 0.5f), Color.red);

            if (result)
            {
                float angle = Vector3.Angle(hit.normal, -gravityNormalized);
                if (angle < groundCheckMaxAngle)
                {
                    GroundNormal = hit.normal;
                    IsGrounded = true;
                    return;
                }
            }

            GroundNormal = Vector3.up;
            IsGrounded = false;
        }

        public void SetStateVelocity(Vector3 velocity) => CurrentVelocity.Write(stateChannelKey, velocity);

        public void SetMovementState(MovementState state)
        {
            for (int i = 0; i < movementStates.Length; i++)
            {
                MovementStateBehavior movementStateBehavior = movementStates[i];
                if (movementStateBehavior.State == state)
                {
                    if (movementStateBehavior.State != CurrentState)
                    {
                        movementStateBehavior.Enter(this);
                    }
                }
                else
                {
                    if (movementStateBehavior.State == CurrentState)
                    {
                        movementStateBehavior.Exit(this);
                    }
                }
            }

            CurrentState = state;
        }

        #region Inputs

        public void ReadInputMove(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            InputDirection = new Vector3(input.x, 0, input.y);
        }

        public void ReadInputJump(InputAction.CallbackContext context)
        {
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