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
        private int currentStateIndex;

        [Header("Ground")]
        [SerializeField] private float groundCheckDistance = 0.1f;
        [SerializeField] private float groundCheckMaxAngle = 50;
        [field: SerializeField] public LayerMask GroundLayer { get; private set; }
        public float DistanceFromGround { get; private set; }

        [Header("Walls")] 
        [SerializeField] private int wallCastSample = 15;
        [SerializeField] private float wallCastDistance = 1.5f;
        [SerializeField] private int wallCastAngle = 60;
        [SerializeField] private float wallrunExitTime = 1;
        [SerializeField] private float wallrunMinHeight = 1.5f;
        private float currentWallrunExitTime;
        private bool exitWallrun;
        public void ExitWallrun()
        {
            exitWallrun = true;
            currentWallrunExitTime = wallrunExitTime;
        }

        [field: SerializeField] public LayerMask WallLayer { get; private set; }
        [field: SerializeField] public bool IsWalled { get; private set; }
        public Vector3 WallNormal { get; private set; }
        public Collider CurrentWall { get; private set; }
        
        [Header("Gravity")]
        [SerializeField] private float gravityScale = 1;
        
        [Header("Coyote")]
        [SerializeField] private int coyoteTime = 10;
        
        [Header("References")]
        [field: SerializeField] public Transform Head { get; private set; }
        [field: SerializeField] public Camera Camera { get; private set; }
        [field: SerializeField, Self, Space] public Rigidbody rb { get; private set; }
        [SerializeField, Child] private CapsuleCollider cc;

        public bool RunInput { get; private set; }
        public bool CrouchInput { get; private set; }
        public bool WantsToWallrun => IsWalled && CurrentWall != null && !exitWallrun && DistanceFromGround > wallrunMinHeight;

        private int jumpInput;
        public bool WantsToJump => jumpInput > 0;
        private int slideInput;
        public bool WantsToSlide => slideInput > 0;

        private ChannelKey stateChannelKey;

        public const float MIN_THRESHOLD = 0.01f;

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
            if (currentStateIndex == -1)
            {
                currentStateIndex = 0;
            }
            
            HandleGroundDetection();
            HandleWallDetection();

            //Set Buffers
            if (jumpInput > 0)
                jumpInput --;
            if (slideInput > 0)
                slideInput --;

            float deltaTime = Time.fixedDeltaTime;
            
            MovementStateBehavior state = movementStates[currentStateIndex];
            Vector3 stateVelocity = state.GetVelocity(this, deltaTime);
            CurrentVelocity.Write(stateChannelKey, stateVelocity);
            
            MovePlayer();
            HandleStateChange();
        }

        private void MovePlayer()
        {
            Vector3 velocity = CurrentVelocity.Value;
            
            /* //Check for incoming collision to prevent player from getting blocked
            Vector3 stateVelocity = StateVelocity;
            if (rb.SweepTest(velocity, out RaycastHit hit, velocity.magnitude * Time.deltaTime))
            {
                CurrentVelocity.Write(stateChannelKey, hit.distance / Time.deltaTime * stateVelocity.normalized);
            }
            */
                     
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
            MovementStateBehavior currentState = movementStates[currentStateIndex];
            MovementState nextState = currentState.GetNextState(this);

            if (nextState != CurrentState)
            {
                SetMovementState(nextState);
            }
        }

        private void HandleWallDetection()
        {
            if (exitWallrun)
            {
                exitWallrun = false;
                
                currentWallrunExitTime -= Time.deltaTime;

                if (currentWallrunExitTime <= 0)
                {
                    exitWallrun = false;
                }

                IsWalled = false;
                WallNormal = Vector3.zero;
                CurrentWall = null;
                return;
            }
            
            Vector3 up = -Gravity.Value.normalized;
            Vector3 castDirection = Vector3.ProjectOnPlane(CurrentVelocity.Value, up).normalized;
            
            for (int i = 0; i < wallCastSample; i++)
            {
                float lerp = Mathf.InverseLerp(0, wallCastSample - 1, i);
                Quaternion rotation = Quaternion.AngleAxis(Mathf.Lerp(-wallCastAngle, wallCastAngle, lerp), up);

                Vector3 direction = rotation * castDirection;
                Debug.DrawRay(transform.position, direction * wallCastDistance, Color.yellow);

                Vector3 p1 = rb.position + cc.center + transform.up * -cc.height * 0.25f;
                Vector3 p2 = p1 + transform.up * cc.height;
                
                if (Physics.CapsuleCast(p1, p2, cc.radius - MIN_THRESHOLD, direction, out RaycastHit hit, wallCastDistance, WallLayer))
                {
                    Debug.DrawRay(hit.point, hit.normal * 10, Color.magenta);
                    
                    float angle = Vector3.Angle(hit.normal, up);
                    if (angle > groundCheckMaxAngle)
                    {
                        IsWalled = true;
                        WallNormal = hit.normal;
                        CurrentWall = hit.collider;
                        return;
                    }
                }
            }
            
            IsWalled = false;
            WallNormal = Vector3.zero;
            CurrentWall = null;
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

            bool distanceResult = Physics.SphereCast(rb.position, cc.radius, gravityNormalized, out RaycastHit hit, Mathf.Infinity, GroundLayer);
            if (distanceResult)
            {
                DistanceFromGround = Vector3.Distance(hit.point, rb.position);
            }
            else
            {
                DistanceFromGround = Mathf.Infinity;
            }
            
            bool result = Physics.SphereCast(rb.position, cc.radius, gravityNormalized, out hit, groundCheckDistance + cc.height * 0.5f - cc.radius, GroundLayer);
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
        
        public void SetMovementState(MovementState state)
        {
            for (int i = 0; i < movementStates.Length; i++)
            {
                MovementStateBehavior movementStateBehavior = movementStates[i];
                if (movementStateBehavior.State == state)
                {
                    if (currentStateIndex != -1)
                    {
                        movementStates[currentStateIndex].Exit(this);
                    }
                    
                    CurrentState = state;
                    currentStateIndex = i;
                    
                    movementStates[currentStateIndex].Enter(this);
                    return;
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
            RunInput = context.performed;
        }

        public void ReadInputCrouch(InputAction.CallbackContext context)
        {
            CrouchInput = context.performed;
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