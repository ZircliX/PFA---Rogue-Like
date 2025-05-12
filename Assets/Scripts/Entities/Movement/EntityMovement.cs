using DG.Tweening;
using KBCore.Refs;
using LTX.ChanneledProperties;
using RogueLike.Player;
using UnityEngine;

namespace DeadLink.Entities.Movement
{
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    public class EntityMovement : MonoBehaviour
    {
        #region Public Properties
        public bool IsGrounded { get; protected set; }
        public bool PreviousIsGrounded { get; protected set; }
        public Vector3 InputDirection { get; protected set; }
        public Vector3 GroundNormal { get; protected set; }
        public Vector3 GroundPosition { get; protected set; }
        public Vector3 StateVelocity => CurrentVelocity.GetValue(stateChannelKey);
        public MovementState CurrentState { get; protected set; }
        public InfluencedProperty<Vector3> CurrentVelocity { get; protected set; }
        public Vector3 Position { get; protected set; }
        public PrioritisedProperty<Vector3> Gravity { get; protected set; }
        #endregion
        
        [Header("Movement States")]
        [SerializeField] protected MovementStateBehavior[] movementStates;
        protected int currentStateIndex;
        
        #region Ground Check

        [Header("Ground Check")]
        [SerializeField] protected float groundCheckDistance = 0.1f;
        [SerializeField] protected float groundCheckMaxAngle = 50;
        [field: SerializeField] public LayerMask GroundLayer { get; private set; }
        public float DistanceFromGround { get; protected set; }

        #endregion
        
        #region Ceiling Check

        [Header("Ceiling Check")]
        [SerializeField] protected float ceilingCheckDistance = 0.1f;
        [SerializeField] protected float ceilingCrouchCheckDistance = 0.1f;
        public bool IsTouchingCeiling { get; protected set; }

        #endregion
        
        [Header("Dash Settings")]
        [SerializeField] protected float dashCooldown = 2.5f;

        #region Walls Detection
        
        [Header("Walls Detection")]
        [SerializeField] protected int wallCastSample = 15;
        [SerializeField] protected float wallCastDistance = 1.5f;
        [SerializeField] protected float wallRunCastExpension = 1.5f;
        [SerializeField] protected int wallCastAngle = 60;
        [SerializeField] protected float wallrunExitTime = 1;
        [SerializeField] protected float wallrunMinHeight = 1.5f;
        [field: SerializeField] public LayerMask WallLayer { get; protected set; }
        public bool IsWalled { get; protected set; }
        public Vector3 WallNormal { get; protected set; }
        public Vector3 LastKnownWallNormal { get; protected set; }
        public Vector3 WallContactPoint { get; protected set; }
        public Collider CurrentWall { get; protected set; }

        #endregion
        
        [Header("Coyote")]
        [SerializeField] protected int coyoteTime = 10;
        [SerializeField] protected float maxYPosition = -35;
        
        #region Gravity

        [Header("Gravity")]
        [SerializeField] protected float gravityScale = 2.75f;
        [SerializeField] protected float gravityAlignSpeed = 5;

        #endregion
        
        #region Height Parameters

        [field: Header("Height")]
        [field: SerializeField] public float BaseCapsuleHeight { get; protected set; } = 2;
        [field: SerializeField] public float CrouchCapsuleHeight { get; protected set; } = 0.5f;
        [field: SerializeField] public float BaseHeadHeight { get; protected set; } = 0.5f;
        [field: SerializeField] public Vector3 ColliderCenterOffset { get; protected set; } = new Vector3(0, -0.5f, 0);
        public PrioritisedProperty<(float, float)> PlayerHeight { get; protected set; }

        #endregion
        
        #region References
        
        [field: Header("References")]
        [field: SerializeField] public Transform CameraTransform { get; private set; }
        [field: SerializeField] public Transform Head { get; private set; }
        [field: SerializeField] public Transform Foot { get; private set; }
        [field: SerializeField, Child] public CapsuleCollider CapsuleCollider { get; private set; }
        [field: SerializeField, Self] public Rigidbody rb { get; private set; }

        #endregion
        
        #region Movement Inputs

        public bool WalkInput { get; protected set; }
        public bool CrouchInput { get; protected set; }

        protected float currentWallrunExitTime;
        public bool CanWallRun()
        {
            bool highOnWall = CurrentWall != null && CurrentWall.bounds.min.y < GetCapsuleBottom().y - 0.25f;

            //Debug.Log($" CanWallRun: {highOnWall} - {IsWalled} - {currentWallrunExitTime} - {DistanceFromGround > wallrunMinHeight}");
            return highOnWall
                   && IsWalled
                   && currentWallrunExitTime <= 0
                   && DistanceFromGround > wallrunMinHeight;
        }

        protected int jumpInput;
        protected bool jumpInputPressed;
        public bool CanJump()
        {
            return jumpInput > 0 && jumpInputPressed;
        }
        
        protected int slideInput;
        public bool CanSlide()
        {
            return slideInput > 0
                && (Position - lastSafePosition).sqrMagnitude > MIN_THRESHOLD * 10;
        }

        public bool DashInput { get; protected set; }
        protected float currentDashCooldown;
        public bool CanDash()
        {
            return DashInput
                   && currentDashCooldown <= 0;
        }

        #endregion
        
        public Vector3 lastSafePosition { get; protected set; }
        protected ChannelKey stateChannelKey;
        protected float stateGravityScale;
        protected bool canChangeGravityScale = true;
        public const float MIN_THRESHOLD = 0.001f;
        protected readonly RaycastHit[] raycastHitsBuffer = new RaycastHit[16];
        protected readonly Collider[] collidersBuffer = new Collider[16];
        
        #region Event Functions
        
        protected virtual void OnValidate() => this.ValidateRefs();

        protected virtual void Awake()
        {
            Position = rb.position;
            CurrentVelocity = new InfluencedProperty<Vector3>(Vector3.zero);
            stateChannelKey = ChannelKey.GetUniqueChannelKey();
            CurrentVelocity.AddInfluence(stateChannelKey, Influence.Add, 1, 0);
            
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
        
        protected virtual void Start() => SetMovementState(MovementState.Idle);
        
        protected virtual void OnDestroy()
        {
            CurrentVelocity.RemoveInfluence(stateChannelKey);
            PlayerHeight.RemovePriority(stateChannelKey);

            for (int i = 0; i < movementStates.Length; i++)
            {
                MovementStateBehavior state = movementStates[i];
                state.Dispose(this);
            }
        }
        
        #endregion
        
        #region Update Functions
        
        protected virtual void Update()
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
            
            //Set Buffers
            if (jumpInput > 0)
                jumpInput--;
            if (slideInput > 0)
                slideInput--;
        }
        
        protected virtual void FixedUpdate()
        {
            Position = rb.position;
            currentStateIndex = currentStateIndex == -1 ? 0 : currentStateIndex;

            HandleGroundDetection();
            HandleCeilingDetection();
            HandleWallDetection();

            float deltaTime = Time.fixedDeltaTime;
            if (canChangeGravityScale)
            {
                stateGravityScale = 1;
            }

            //Manage States Values
            MovementStateBehavior state = movementStates[currentStateIndex];

            Vector3 stateVelocity = state.GetVelocity(this, deltaTime, ref stateGravityScale);
            stateVelocity += Gravity.Value * (stateGravityScale * gravityScale * deltaTime);
            CurrentVelocity.Write(stateChannelKey, stateVelocity);

            MovePlayer();
            HandleGravityOrientation();
            HandleStateChange();

            HandleVoidDetection();
        }
        
        #endregion
        
        private void MovePlayer()
        {
            if (IsGrounded)
            {
                Plane plane = new Plane(GroundNormal, GroundPosition + GroundNormal * 0.02f);

                Vector3 capsuleBottom = GetCapsuleBottom();
                Vector3 snapPosition = plane.ClosestPointOnPlane(capsuleBottom);

                Vector3 deltaPosition = snapPosition - capsuleBottom;
                if (deltaPosition.sqrMagnitude > 0.1f)
                {
                    rb.DOMove(rb.position + deltaPosition, 0.1f);
                }
            }

            Vector3 velocity = CurrentVelocity.Value;
            rb.linearVelocity = velocity;
        }
        
        private void HandleGravityOrientation()
        {
            Vector3 targetUp = -Gravity.Value.normalized;
            Vector3 right = CameraTransform.right;
            Vector3 forward = Vector3.Cross(right, targetUp);

            Quaternion rotation = Quaternion.LookRotation(forward, targetUp);

            rb.MoveRotation(Quaternion.Slerp(rb.rotation, rotation, gravityAlignSpeed * Time.deltaTime));
        }
        
        #region Detections
        
        protected virtual void HandleVoidDetection()
        {
            if (IsGrounded)
            {
                lastSafePosition = Position;
            }
        }

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
                // Debug.Log(Vector3.Dot(closestHit.normal, currentVelocityValue));
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
            
            /*
            Debug.DrawRay(ccPos, upDirection * (checkDistance + MIN_THRESHOLD),
                Color.blue);
            */

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

            PreviousIsGrounded = IsGrounded;
            Vector3 rayDirection = Gravity.Value.normalized;

            //Ground Check
            float radius = CapsuleCollider.radius - MIN_THRESHOLD;
            float height = CapsuleCollider.height * .5f;

            Vector3 center = GetCapsuleTop() - GetCapsuleOrientation() * CapsuleCollider.radius;
            float castDistance = height + groundCheckDistance;

            int size = Physics.SphereCastNonAlloc(
                center, radius, rayDirection,
                raycastHitsBuffer,
                castDistance, GroundLayer);

            int overlapSize = Physics.OverlapSphereNonAlloc(center, radius, collidersBuffer, GroundLayer);
            // Debug.DrawLine(center, center + rayDirection * height, Color.cyan);
            // Debug.DrawRay(center, rayDirection * (castDistance + radius), Color.cyan);
            RaycastHit closestHit = default;

            for (int i = 0; i < size; i++)
            {
                RaycastHit hit = raycastHitsBuffer[i];

                if (closestHit.colliderInstanceID == 0 || closestHit.distance > hit.distance)
                {
                    float angle = Vector3.Angle(hit.normal, -rayDirection);
                    if (angle > groundCheckMaxAngle)
                        continue;

                    bool isValid = true;
                    for (int j = 0; j < overlapSize; j++)
                    {
                        if (collidersBuffer[j] == hit.collider)
                        {
                            // Debug.Log($"Overlapping with {hit.collider.name}");
                            isValid = false;
                            break;
                        }
                    }
                    if(!isValid)
                        continue;

                    // Debug.Log(hit.collider.name, hit.collider);
                    // Debug.DrawRay(hit.point, hit.normal * .1f, Color.magenta, 1);
                    // Debug.DrawLine(hit.point, center, Color.magenta, 1);
                    closestHit = hit;
                }
            }

            if (closestHit.colliderInstanceID != 0)
            {
                GroundNormal = closestHit.normal;
                GroundPosition = closestHit.point;
                IsGrounded = true;
                DistanceFromGround = closestHit.distance;
                return;
            }

            GroundNormal = Vector3.up;
            IsGrounded = false;

            if (PreviousIsGrounded)
            {
                jumpInput = coyoteTime;
                Debug.Log(jumpInput);
            }

            GroundPosition = Position;
            DistanceFromGround = Mathf.Infinity;
        }

        #endregion
        
        #region Capsule Extensions
        
        public Vector3 GetCapsuleOrientation() => CapsuleCollider.direction switch
        {
            1 => transform.up,
            2 => transform.right,
            3 => transform.forward,
            _ => Vector3.zero
        };

        public Vector3 GetCapsuleCenter() => Position + CapsuleCollider.center;
        public Vector3 GetCapsuleBottom()
        {
            Vector3 capsuleCenter = GetCapsuleCenter();
            if (CapsuleCollider.height > CapsuleCollider.radius * 2)
            {
                return capsuleCenter - GetCapsuleOrientation() * (CapsuleCollider.height * .5f);
            }

            return capsuleCenter - GetCapsuleOrientation() * CapsuleCollider.radius;
        }

        public Vector3 GetCapsuleTop()
        {
            Vector3 capsuleCenter = GetCapsuleCenter();
            if (CapsuleCollider.height > CapsuleCollider.radius * 2)
            {
                return capsuleCenter + GetCapsuleOrientation() * (CapsuleCollider.height * .5f);
            }

            return capsuleCenter + GetCapsuleOrientation() * CapsuleCollider.radius;
        }
        
        #endregion
        
        #region States

        protected void HandleStateChange()
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

                    //Debug.Log($"Enter state : {CurrentState}");
                    movementStateBehavior.Enter(this);

                    (float newCapsuleHeight, float newHeadHeight) = movementStateBehavior.GetHeight(this);
                    PlayerHeight.Write(stateChannelKey, (newCapsuleHeight, newHeadHeight));
                    return;
                }
            }
        }

        #endregion
        
        #region Movement

        protected void SetPlayerHeight(float newCapsuleHeight, float newHeadHeight)
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
    }
}