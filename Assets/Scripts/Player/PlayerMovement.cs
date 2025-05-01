using System;
using DeadLink.Cameras;
using DeadLink.Extensions;
using DG.Tweening;
using KBCore.Refs;
using LTX.ChanneledProperties;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RogueLike.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        #region Public Properties
        public bool IsGrounded { get; private set; }
        public Vector3 InputDirection { get; private set; }
        public Vector3 GroundNormal { get; private set; }
        public Vector3 StateVelocity => CurrentVelocity.GetValue(stateChannelKey);
        public MovementState CurrentState { get; private set; }
        public InfluencedProperty<Vector3> CurrentVelocity { get; private set; }
        public Vector3 Position => rb.position;
        public PrioritisedProperty<Vector3> Gravity { get; private set; }
        
        #endregion

        [Header("Movement States")] 
        [SerializeField] private MovementStateBehavior[] movementStates;
        private int currentStateIndex;

        #region Ground Check

        [Header("Ground Check")] 
        [SerializeField] private float groundCheckDistance = 0.1f;
        [SerializeField] private float groundCheckMaxAngle = 50;
        [field: SerializeField] public LayerMask GroundLayer { get; private set; }
        public float DistanceFromGround { get; private set; }

        #endregion

        #region Ceiling Check

        [Header("Ceiling Check")] 
        [SerializeField] private float ceilingCheckDistance = 0.1f;
        [SerializeField] private float ceilingCrouchCheckDistance = 0.1f;
        public bool IsTouchingCeiling { get; private set; }

        #endregion

        #region Walls Detection
        
        [Header("Dash Settings")]
        [SerializeField] private float dashCooldown = 2.5f;

        [Header("Walls Detection")] 
        [SerializeField] private int wallCastSample = 15;
        [SerializeField] private float wallCastDistance = 1.5f;
        [SerializeField] private float wallRunCastExpension = 1.5f;
        [SerializeField] private int wallCastAngle = 60;
        [SerializeField] private float wallrunExitTime = 1;
        [SerializeField] private float wallrunMinHeight = 1.5f;
        [field: SerializeField] public LayerMask WallLayer { get; private set; }
        public bool IsWalled { get; private set; }
        public Vector3 WallNormal { get; private set; }
        public Vector3 LastKnownWallNormal { get; private set; }
        public Vector3 WallContactPoint { get; private set; }
        public Collider CurrentWall { get; private set; }

        #endregion
        
        #region Power ups
        
        private int remainingJump = 1;
        private int remainingDash = 1;
        public void AddBonusJump(int value) => remainingJump += value;
        public void AddBonusDash(int value) => remainingDash += value;
        
        #endregion

        #region Gravity

        [Header("Gravity")] 
        [SerializeField] private float gravityScale = 2.75f;
        [SerializeField] private float gravityAlignSpeed = 5;

        #endregion

        [Header("Coyote")] 
        [SerializeField] private int coyoteTime = 10;
        [SerializeField] private float maxYPosition = -35;

        #region Height Parameters

        [field: Header("Height")]
        [field: SerializeField] public float BaseCapsuleHeight { get; private set; } = 2;
        [field: SerializeField] public float CrouchCapsuleHeight { get; private set; } = 0.5f;
        [field: SerializeField] public float BaseHeadHeight { get; private set; } = 0.5f;
        [field: SerializeField] public Vector3 ColliderCenterOffset { get; private set; } = new Vector3(0, -0.5f, 0);
        public PrioritisedProperty<(float, float)> PlayerHeight { get; private set; }

        #endregion

        #region References

        [field: Header("References")]
        [field: SerializeField] public Camera Camera { get; private set; }
        [field: SerializeField] public Transform Head { get; private set; }
        [field: SerializeField] public Transform Foot { get; private set; }
        [field: SerializeField, Child] public CapsuleCollider CapsuleCollider { get; private set; }
        [field: SerializeField, Self] public Rigidbody rb { get; private set; }

        #endregion

        #region Movement Inputs
        
        public bool WalkInput { get; private set; }
        public bool CrouchInput { get; private set; }

        public bool WantsToWallrun => IsWalled 
                                      && CurrentWall != null
                                      && DistanceFromGround > wallrunMinHeight;
        private float currentWallrunExitTime;

        private int jumpInput;
        public bool WantsToJump => jumpInput > 0;
        private int slideInput;
        public bool WantsToSlide => slideInput > 0;

        private bool dashInput;
        public bool WantsToDash => remainingDash > 0 
                                   && dashInput 
                                   && currentDashCooldown <= 0;
        private float currentDashCooldown;
        
        #endregion

        private Vector3 lastSafePosition;
        private ChannelKey stateChannelKey;
        public const float MIN_THRESHOLD = 0.001f;
        
        private void OnValidate() => this.ValidateRefs();

        private void Awake()
        {
            CurrentVelocity = new InfluencedProperty<Vector3>(Vector3.zero);
            stateChannelKey = ChannelKey.GetUniqueChannelKey();
            CurrentVelocity.AddInfluence(stateChannelKey, Influence.Add, 1, 0);
            CameraController.Instance.CameraEffectProperty.AddPriority(stateChannelKey, PriorityTags.High);

            PlayerHeight = new PrioritisedProperty<(float, float)>((BaseCapsuleHeight, BaseHeadHeight));
            PlayerHeight.AddPriority(stateChannelKey, PriorityTags.Default);
            PlayerHeight.AddOnValueChangeCallback(ctx =>
            {
                SetPlayerHeight(ctx.Item1, ctx.Item2);
            }, true);

            Gravity = new PrioritisedProperty<Vector3>(Vector3.down * 9.81f);

            for (int i = 0; i < movementStates.Length; i++)
            {
                MovementStateBehavior state = movementStates[i];
                state.Initialize(this);
            }
        }

        private void Start() => SetMovementState(MovementState.Walking);
        
        private void OnDestroy()
        {
            CurrentVelocity.RemoveInfluence(stateChannelKey);
            PlayerHeight.RemovePriority(stateChannelKey);

            for (int i = 0; i < movementStates.Length; i++)
            {
                MovementStateBehavior state = movementStates[i];
                state.Dispose(this);
            }
        }
        
        private void Update()
        {
            //Wallrun Cooldown
            if (currentWallrunExitTime > 0)
                currentWallrunExitTime -= Time.deltaTime;
            else
                currentWallrunExitTime = 0;

            //Dash Cooldown
            if (currentDashCooldown > 0)
                currentDashCooldown -= Time.deltaTime;
            else
                currentDashCooldown = 0;
            
            HandleGroundDetection();
            HandleCeilingDetection();
            HandleWallDetection();
        }

        private void FixedUpdate()
        {
            currentStateIndex = currentStateIndex == -1 ? 0 : currentStateIndex;

            //Set Buffers
            if (jumpInput > 0)
                jumpInput--;
            if (slideInput > 0)
                slideInput--;

            float deltaTime = Time.fixedDeltaTime;
            float stateGravityScale = 1;

            //Manage States Values
            MovementStateBehavior state = movementStates[currentStateIndex];
            
            Vector3 stateVelocity = state.GetVelocity(this, deltaTime, ref stateGravityScale);
            stateVelocity += Gravity.Value * (stateGravityScale * gravityScale * deltaTime);
            CurrentVelocity.Write(stateChannelKey, stateVelocity);
            
            CameraController.Instance.CameraEffectProperty.Write(stateChannelKey,
                state.GetCameraEffects(this, Time.deltaTime));

            MovePlayer();
            HandleGravityOrientation();
            HandleStateChange();
            
            HandleVoidDetection();
        }

        private void HandleVoidDetection()
        {
            if (Position.y < - Mathf.Abs(maxYPosition))
            {
                this.TeleportPlayer(lastSafePosition, 1, 2);
            }

            if (IsGrounded)
            {
                lastSafePosition = Position;
            }
        }

        private void HandleGravityOrientation()
        {
            Vector3 targetUp = -Gravity.Value.normalized;
            Vector3 right = Camera.transform.right;
            Vector3 forward = Vector3.Cross(right, targetUp);

            Quaternion rotation = Quaternion.LookRotation(forward, targetUp);

            rb.MoveRotation(Quaternion.Slerp(rb.rotation, rotation, gravityAlignSpeed * Time.deltaTime));
        }

        private void MovePlayer()
        {
            Vector3 velocity = CurrentVelocity.Value;
            rb.linearVelocity = velocity;
        }

        #region States

        private void HandleStateChange()
        {
            MovementStateBehavior currentState = movementStates[currentStateIndex];
            MovementState nextState = currentState.GetNextState(this);

            if (nextState != CurrentState)
            {
                //Debug.Log($"Current State: {CurrentState} => Next State: {nextState}");
                SetMovementState(nextState);
            }
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

                    movementStateBehavior.Enter(this);

                    (float newCapsuleHeight, float newHeadHeight) = movementStateBehavior.GetHeight(this);
                    PlayerHeight.Write(stateChannelKey, (newCapsuleHeight, newHeadHeight));
                    return;
                }
            }
        }
        
        #endregion
        
        #region Detections

        private void HandleWallDetection()
        {
            /*
            if (currentWallrunExitTime > 0)
            {
                IsWalled = false;
                WallNormal = Vector3.zero;
                CurrentWall = null;
                return;
            }
            */

            Vector3 up = -Gravity.Value.normalized;
            Vector3 castDirection = Vector3.ProjectOnPlane(CurrentVelocity.Value, up).normalized;

            RaycastHit closestHit = default;
            Vector3 currentVelocityValue = CurrentVelocity.Value.normalized;
            
            for (int i = 0; i < wallCastSample; i++)
            {
                float lerp = Mathf.InverseLerp(0, wallCastSample - 1, i);
                Quaternion rotation = Quaternion.AngleAxis(Mathf.Lerp(-wallCastAngle, wallCastAngle, lerp), up);

                Vector3 direction = rotation * castDirection;

                float halfHeight = CapsuleCollider.height * 0.25f;
                float radius = CapsuleCollider.radius;

                Vector3 center = Position + CapsuleCollider.center;
                Vector3 p1 = center + transform.up * -halfHeight;
                Vector3 p2 = center + transform.up * halfHeight;

                float dist = CurrentState == MovementState.WallRunning
                    ? wallCastDistance + wallRunCastExpension
                    : wallCastDistance;
                
                Debug.DrawRay(transform.position, direction * dist, Color.yellow);
                if (Physics.CapsuleCast(p1, p2, radius - MIN_THRESHOLD, direction, out RaycastHit hit,
                        dist, WallLayer))
                {
                    float angle = Vector3.Angle(hit.normal, up);
                    if (angle > groundCheckMaxAngle && Vector3.Dot(hit.normal, currentVelocityValue) < 0.01f)
                    {
                        if (closestHit.collider == null || closestHit.distance > hit.distance)
                        {
                            closestHit = hit;
                        }
                    }
                }
            }

            if (closestHit.collider != null)
            {
                Debug.Log(Vector3.Dot(closestHit.normal, currentVelocityValue));
                IsWalled = true;
                LastKnownWallNormal = WallNormal;
                WallNormal = closestHit.normal;
                WallContactPoint = closestHit.point;
                CurrentWall = closestHit.collider;
                return;
            }

            IsWalled = false;
            WallNormal = Vector3.zero;
            CurrentWall = null;
        }
        
        private void HandleCeilingDetection()
        {
            Vector3 upDirection = -Gravity.Value.normalized;
            Vector3 ccPos = transform.TransformPoint(CapsuleCollider.center);
            
            float checkDistance = CurrentState is MovementState.Crouching or MovementState.Sliding
                ? ceilingCrouchCheckDistance
                : ceilingCheckDistance;
            
            bool result = Physics.SphereCast(ccPos, CapsuleCollider.radius, upDirection, out RaycastHit hit,
                checkDistance + MIN_THRESHOLD);
                
            Debug.DrawRay(ccPos, upDirection * (checkDistance + MIN_THRESHOLD),
                Color.blue);

            IsTouchingCeiling = result;
        }

        private void HandleGroundDetection()
        {
            if (CurrentState == MovementState.Jumping && StateVelocity.y >= 0)
            {
                GroundNormal = Vector3.up;
                IsGrounded = false;
                return;
            }

            Vector3 rayDirection = Gravity.Value.normalized;
            
            //Ground Check
            float radius = CapsuleCollider.radius;
            Vector3 center = Foot.position - Gravity.Value.normalized * (radius + MIN_THRESHOLD);
            float castDistance = groundCheckDistance + MIN_THRESHOLD;
            
            RaycastHit[] hitsBuffer = Physics.SphereCastAll(center, radius, rayDirection, castDistance, 
                GroundLayer, QueryTriggerInteraction.Ignore);

            int hitCount = hitsBuffer.Length;

            Debug.DrawRay(center, rayDirection * (castDistance + radius), Color.cyan);
            Debug.Log($"Count = {hitCount}, Distance = {castDistance}, center = {center}, radius = {radius}");
            
            RaycastHit closestHit = default;
            for (int i = 0; i < hitCount; i++)
            {
                RaycastHit hit = hitsBuffer[i];
                
                if (closestHit.collider == null || closestHit.distance > hit.distance)
                {
                    float angle = Vector3.Angle(hit.normal, -rayDirection);
                    if (angle < groundCheckMaxAngle)
                    {
                        Debug.Log(hit.collider.name, hit.collider);
                        Debug.DrawRay(hit.point, hit.normal * 10, Color.magenta);
                        Debug.DrawLine(hit.point, Position, Color.magenta);
                        closestHit = hit;
                    }
                }
            }

            if (closestHit.collider != null)
            {
                GroundNormal = closestHit.normal;
                IsGrounded = true;
                DistanceFromGround = closestHit.distance;
                return;
            }
            
            GroundNormal = Vector3.up;
            IsGrounded = false;
            DistanceFromGround = Mathf.Infinity;
        }
        
        #endregion
        
        #region Movement
        
        private void SetPlayerHeight(float newCapsuleHeight, float newHeadHeight)
        {
            Vector3 targetCenter = Mathf.Approximately(newCapsuleHeight, BaseCapsuleHeight)
                ? new Vector3(0, 0, 0)
                : ColliderCenterOffset;

            CapsuleCollider.height = newCapsuleHeight;
            CapsuleCollider.center = targetCenter;
            
            Vector3 targetHead = new Vector3(Head.localPosition.x, newHeadHeight, Head.localPosition.z);
            Head.DOLocalMove(targetHead, 0.25f).SetEase(Ease.OutCubic);
        }
        
        public void ExitWallrun()
        {
            currentWallrunExitTime = wallrunExitTime;
        }

        public void DashCooldown()
        {
            currentDashCooldown = dashCooldown;
        }
        
        #endregion

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
            WalkInput = context.performed;
        }

        public void ReadInputCrouch(InputAction.CallbackContext context)
        {
            CrouchInput = context.performed;
        }

        public void ReadInputSlide(InputAction.CallbackContext context)
        {
            //Debug.Log("SLIDE");
            
            if (context.performed)
            {
                slideInput = coyoteTime;
            }
            else if (context.canceled)
            {
                slideInput = 0;
            }
        }

        public void ReadInputDash(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                //Debug.Log("Dash = true");
                dashInput = true;
            }
            else if (context.canceled)
            {
                dashInput = false;
            }
        }

        #endregion

    }
}