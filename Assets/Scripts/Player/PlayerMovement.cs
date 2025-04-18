using System.Collections.Generic;
using DeadLink.PowerUpSystem;
using DeadLink.PowerUpSystem.InterfacePowerUps;
using KBCore.Refs;
using LTX.ChanneledProperties;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RogueLike.Player
{
    public class PlayerMovement : MonoBehaviour, IVisitable
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

        #region Walls Detection

        [Header("Walls Detection")] 
        [SerializeField] private int wallCastSample = 15;
        [SerializeField] private float wallCastDistance = 1.5f;
        [SerializeField] private int wallCastAngle = 60;
        [SerializeField] private float wallrunExitTime = 1;
        [SerializeField] private float wallrunMinHeight = 1.5f;
        [field: SerializeField] public LayerMask WallLayer { get; private set; }
        [field: SerializeField] public bool IsWalled { get; private set; }
        public Vector3 WallNormal { get; private set; }
        public Collider CurrentWall { get; private set; }
        private float currentWallrunExitTime;
        private bool exitWallrun;

        #endregion
        
        #region Power ups

        private Dictionary<string, IVisitor> unlockedPowerUps;

        private int remainingJump;
        private int remainingDash;
        public void AddBonusJump(int value) => remainingJump += value;
        public void AddBonusDash(int value) => remainingDash += value;

        public void Unlock(IVisitor visitor)
        {
            visitor.OnBeUnlocked(this);
            unlockedPowerUps.Add(visitor.Name, visitor);
        }

        public void Use(string powerUpName)
        {
            unlockedPowerUps[powerUpName].OnBeUsed(this);
        }

        #endregion

        #region Gravity

        [Header("Gravity")] 
        [SerializeField] private float gravityScale = 1;
        [SerializeField] private float gravityAlignSpeed = 5;

        #endregion

        [Header("Coyote")] 
        [SerializeField] private int coyoteTime = 10;

        #region Height Parameters

        [field: Header("Height")]
        [field: SerializeField] public float BaseCapsuleHeight { get; private set; } = 2;
        [field: SerializeField] public float BaseHeadHeight { get; private set; } = 0.5f;
        [field: SerializeField] public Vector3 ColliderCenterOffset { get; private set; } = new Vector3(0, -0.5f, 0);
        public PrioritisedProperty<(float, float)> PlayerHeight { get; private set; }

        #endregion

        #region References

        [field: Header("References")]
        [field: SerializeField] public Camera Camera { get; private set; }
        [field: SerializeField] public Transform Head { get; private set; }
        [field: SerializeField, Child] public CapsuleCollider CapsuleCollider { get; private set; }
        [field: SerializeField, Self] public Rigidbody rb { get; private set; }

        #endregion

        #region Movement Inputs
        
        public bool RunInput { get; private set; }
        public bool CrouchInput { get; private set; }

        public bool WantsToWallrun => IsWalled &&
                                      CurrentWall != null
                                      && !exitWallrun
                                      && DistanceFromGround > wallrunMinHeight;

        private int jumpInput;
        public bool WantsToJump => jumpInput > 0;
        private int slideInput;
        public bool WantsToSlide => slideInput > 0;
        
        #endregion

        private ChannelKey stateChannelKey;
        public const float MIN_THRESHOLD = 0.01f;

        private void OnValidate() => this.ValidateRefs();

        private void Awake()
        {
            unlockedPowerUps = new Dictionary<string, IVisitor>();
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

        private void OnEnable() => VisitableReferenceManager.Instance.RegisterComponent(GameMetrics.Global.PlayerMovementVisitableType, this);

        private void OnDisable() => VisitableReferenceManager.Instance.UnregisterComponent(GameMetrics.Global.PlayerMovementVisitableType);

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

            MovePlayer();
            HandleGravityOrientation();
            HandleStateChange();
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
            /*
            if (rb.isKinematic)
            {
                float deltaTime = Time.deltaTime;
                Vector3 p1 = rb.position + cc.center + transform.up * (cc.height * 0.25f);
                Vector3 p2 = p1 + transform.up * cc.height;

                RaycastHit[] raycastHits = new RaycastHit[16];
                Vector3 collisionOffset = Vector3.zero;

                for (int i = 0; i < maxPenetrationCount; i++)
                {
                    Vector3 nextPosition = rb.position + collisionOffset + velocity * deltaTime;
                    int count = Physics.CapsuleCastNonAlloc(p1, p2, cc.radius - MIN_THRESHOLD, velocity.normalized, raycastHits, velocity.magnitude);

                    if (count > 0)
                    {
                        for (int j = 0; j < count; j++)
                        {
                            RaycastHit hit = raycastHits[j];
                            Collider hitCollider = hit.collider;

                            if (hit.collider.attachedRigidbody == rb)
                                continue;

                            if (Physics.GetIgnoreLayerCollision(hit.collider.gameObject.layer, cc.gameObject.layer))
                                continue;

                            if (Physics.GetIgnoreCollision(hit.collider, cc))
                                continue;

                            Vector3 otherPosition = hitCollider.transform.position;
                            Quaternion otherRotation = hitCollider.transform.rotation;

                            if (Physics.ComputePenetration(
                                    cc, nextPosition, rb.rotation,
                                    hitCollider, otherPosition, otherRotation,
                                    out Vector3 direction, out float distance))
                            {
                                Vector3 offset = direction * distance;
                                Debug.DrawLine(rb.position + collisionOffset, rb.position + collisionOffset + offset, Color.red);
                                collisionOffset += offset;
                            }
                        }
                    }
                }

                rb.MovePosition(rb.position + velocity * deltaTime + collisionOffset);
            }
            else
            {
                Vector3 velocity = CurrentVelocity.Value;
                rb.linearVelocity = velocity;
            }
            */

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

                Vector3 p1 = rb.position + CapsuleCollider.center + transform.up * -CapsuleCollider.height * 0.25f;
                Vector3 p2 = p1 + transform.up * CapsuleCollider.height;

                if (Physics.CapsuleCast(p1, p2, CapsuleCollider.radius - MIN_THRESHOLD, direction, out RaycastHit hit,
                        wallCastDistance, WallLayer))
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

            bool distanceResult = Physics.SphereCast(rb.position, CapsuleCollider.radius, gravityNormalized,
                out RaycastHit hit, Mathf.Infinity, GroundLayer);
            if (distanceResult)
            {
                DistanceFromGround = Vector3.Distance(hit.point, rb.position);
            }
            else
            {
                DistanceFromGround = Mathf.Infinity;
            }

            Vector3 ccPos = transform.TransformPoint(CapsuleCollider.center);
            bool result = Physics.SphereCast(ccPos, CapsuleCollider.radius, gravityNormalized, out hit,
                groundCheckDistance + CapsuleCollider.height * 0.5f + MIN_THRESHOLD, GroundLayer);
            Debug.DrawRay(ccPos, gravityNormalized * (groundCheckDistance + CapsuleCollider.height * 0.5f + MIN_THRESHOLD),
                Color.red);

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
        
        #endregion
        
        #region Movement
        
        private void SetPlayerHeight(float newCapsuleHeight, float newHeadHeight)
        {
            CapsuleCollider.height = newCapsuleHeight;
            CapsuleCollider.center = Mathf.Approximately(newCapsuleHeight, BaseCapsuleHeight)
                ? new Vector3(0, 0, 0)
                : ColliderCenterOffset;
            Head.localPosition = new Vector3(Head.localPosition.x, newHeadHeight, Head.localPosition.z);
        }
        
        public void ExitWallrun()
        {
            exitWallrun = true;
            currentWallrunExitTime = wallrunExitTime;
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
            RunInput = context.performed;
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

        #endregion
        
    }
}