using System;
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
        [field: SerializeField] public MovementState CurrentState { get; private set; }
        public InfluencedProperty<Vector3> CurrentVelocity { get; private set; }
        public PrioritisedProperty<Vector3> Gravity { get; private set; }

        [SerializeField] private float groundCheckDistance;
        [SerializeField] private float groundCheckRadius;
        [SerializeField] private float groundCheckMaxAngle;
        [SerializeField] private LayerMask groundLayer;

        [SerializeField] private float gravityScale = 1;
        [SerializeField] private int coyoteTime = 10;
        [SerializeField, Self] private Rigidbody rb;
        [SerializeField, Child] private CapsuleCollider cc;

        [SerializeField] private MovementStateBehavior[] movementStates;
        
        private bool runInput;
        private int jumpInput;
        public bool WantsToJump => jumpInput > 0;
        private int slideInput;
        private bool WantsToSlide => slideInput > 0;
        private bool crouchInput;

        private ChannelKey stateChannelKey;

        private void OnValidate()
        {
            this.ValidateRefs();
        }

        private void Awake()
        {
            CurrentVelocity = new InfluencedProperty<Vector3>(Vector3.zero);
            CurrentVelocity.AddInfluence(this, Influence.Add, 0, 0);

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
            
            //Debug.Log(jumpInput);
            
            //Set Buffers
            if (jumpInput > 0)
            {
                jumpInput --;
            }
            if (slideInput > 0)
                slideInput --;
            
            float deltaTime = Time.deltaTime;
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
            
            //Debug.Log(rb.linearVelocity);
        }

        private void MovePlayer()
        {
            Vector3 currentGravityVelocity = IsGrounded ? - GroundNormal : CurrentVelocity[this] + Gravity.Value * Time.deltaTime;
            CurrentVelocity.Write(this, currentGravityVelocity * gravityScale);
            
            rb.linearVelocity = CurrentVelocity;
        }

        private void HandleStateChange()
        {
            switch (CurrentState)
            {
                case MovementState.Falling:
                    if (IsGrounded)
                    {
                        SetMovementState(runInput ? MovementState.Running : MovementState.Walking);
                    }
                    break;
                case MovementState.Jumping:
                    if (CurrentVelocity.Value.y < 0)
                    {
                        SetMovementState(MovementState.Falling);
                    }
                    break;
                case MovementState.Idle:
                    if (WantsToJump)
                    {
                        SetMovementState(MovementState.Jumping);
                        jumpInput = 0;
                    }
                    break;
                case MovementState.Crouching:
                    break;
                case MovementState.Walking:
                case MovementState.Running:
                    if (!IsGrounded)
                    {
                        SetMovementState(MovementState.Falling);
                    }
                    else
                    {
                        if (WantsToJump)
                        {
                            SetMovementState(MovementState.Jumping);
                            jumpInput = 0;
                        }
                        else
                        {
                            SetMovementState(runInput ? MovementState.Running : MovementState.Walking);
                        }
                    }
                    break;
                case MovementState.Sliding:
                    if (WantsToJump)
                    {
                        SetMovementState(MovementState.Jumping);
                        jumpInput = 0;
                    }
                    break;
            }
        }
        
        private void HandleGroundDetection()
        {
            Vector3 gravityNormalized = Gravity.Value.normalized;
            
            bool result = Physics.SphereCast(rb.position, groundCheckRadius,
                gravityNormalized, out RaycastHit hit, groundCheckDistance + cc.height * 0.5f - groundCheckRadius, groundLayer);
            
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

        private void SetMovementState(MovementState state)
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