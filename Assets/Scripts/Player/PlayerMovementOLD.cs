using System.Collections;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RogueLike.Player
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class PlayerMovementOLD : MonoBehaviour
    {
        /*
       [Header("Movement")]
       private float moveSpeed;
       private Vector3 moveDirection;
       public float slidingForce;
       public bool sliding;
       private float desiredMoveSpeed;
       private float lastDesiredMoveSpeed;
       private bool runInput;
       private bool crouchInput;

       [Header("Jump")]
       private float nbrJumps;
       private bool readyToJump;
       private bool jumpInput;

       [Header("Slopes")]
       private RaycastHit slopeHit;
       private bool exitSlope;

       [Header("Wall Runs")]
       public bool wallRunning;
       public bool exitWallRun;

       [Header("Sliding")]
       [SerializeField] private float maxSlideTime;
       [SerializeField] private float slideForce;
       public float slideYScale;
       private float slideTimer;
       private float startYScale;

       [Header("Input")]
       public KeyCode slideKey = KeyCode.LeftControl;
       private float horizontalInput;
       private float verticalInput;

       [Header("Stamina")]
       public float maxStamina;
       [SerializeField] private float staminaCost;
       [SerializeField] private float staminaRegen;

       [HideInInspector] public float stamina;
       [HideInInspector] public bool isReadyToRegen;
       [HideInInspector] public bool isReadyToSprint;
       [HideInInspector] public bool isReadyToSlide;

       [Header("Params")]
       [SerializeField, Self] private Rigidbody rb;
       private bool grounded;
       private Vector2 inputDirection;
       [HideInInspector] public Vector3 flatVel;
       [HideInInspector] public Vector3 vel;

       private Transform orientation => PlayerController.Instance.Orientation;

       private MovementState state;
       private enum MovementState
       {
           walking,
           sprinting,
           crouching,
           sliding,
           air,
           wallRunning
       }

       private void OnValidate()
       {
           this.ValidateRefs();
       }

       public void ReadInputMove(InputAction.CallbackContext context)
       {
           inputDirection = context.ReadValue<Vector2>();
       }

       public void ReadInputJump(InputAction.CallbackContext context)
       {
           jumpInput = context.performed;
       }

       public void ReadInputRun(InputAction.CallbackContext context)
       {
           runInput = context.performed;
       }

       public void ReadInputCrouch(InputAction.CallbackContext context)
       {
           crouchInput = context.performed;
       }

       private void Start()
       {
           readyToJump = true;
           AudioManager.Global.PlayOneShot(GameMetrics.Global.test, transform.position);

           StartStamina();
           StartSliding();
       }

       private void Update()
       {
           grounded = Physics.Raycast(transform.position, Vector3.down, GameMetrics.Global.PlayerHeight * 0.5f + 0.2f, GameMetrics.Global.GroundLayer);

           MyInput();
           SpeedControl();
           StateHandler();
           DoVel();

           UpdateStamina();
           UpdateSliding();

           if (grounded)
           {
               rb.linearDamping = GameMetrics.Global.GroundDrag;
               nbrJumps = GameMetrics.Global.MaxJumps;
           }
           else
           {
               rb.linearDamping = 0;
           }

           if (vel.magnitude < 5.5)
           {
               //stamina.IncreaseStamina(true);
           }
           else if (state == MovementState.walking)
           {
               //stamina.IncreaseStamina(false);
           }
       }

       private void FixedUpdate()
       {
           MovePlayer();
           if (sliding)
           {
               SlidingMovement();
           }
       }

       #region Sliding

       private void StartSliding()
       {
           startYScale = PlayerController.Instance.PlayerModel.localScale.y;
       }

       private void UpdateSliding()
       {
           if (Input.GetKeyDown(slideKey) && isReadyToSlide && isReadyToSprint && (horizontalInput != 0 || verticalInput != 0))
               StartSlide();

           if (Input.GetKeyUp(slideKey) && sliding)
               StopSlide();
       }

       private void StartSlide()
       {
           sliding = true;
           SlideStamina();
           slidingForce = 4.5f;


           rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

           slideTimer = maxSlideTime;
       }

       private void SlidingMovement()
       {
           // sliding normal
           if(!OnSlope() || rb.linearVelocity.y > -0.1f)
           {
               slideTimer -= Time.deltaTime;
           }

           if (slideTimer <= 0)
               StopSlide();
       }

       private void StopSlide()
       {
           sliding = false;
           slidingForce = 0f;

           Transform playerModel = PlayerController.Instance.PlayerModel;
           playerModel.localScale = new Vector3(playerModel.localScale.x, startYScale, playerModel.localScale.z);
       }

       #endregion

       #region Stamina

       private void StartStamina()
       {
           stamina = maxStamina;
           isReadyToRegen = false;
           isReadyToSprint = true;
           isReadyToSlide = true;
       }

       private void UpdateStamina()
       {
           if (stamina > maxStamina-2)
           {
               stamina = maxStamina;
               isReadyToRegen = false;
           }

           if (stamina <= 1){
               isReadyToRegen = true;
               isReadyToSprint = false;
           }
           if (stamina >= 250){
               isReadyToSprint = true;
           }
           if (stamina >= 100){
               isReadyToSlide = true;
           }
           else {isReadyToSlide = false;}
       }

       public void IncreaseStamina(bool idle)
       {
           if (isReadyToRegen || idle){
               stamina += staminaRegen * Time.deltaTime;
           }
       }

       public void AddStaminaPowers(float number)
       {
           stamina += number;
       }

       public void DecreaseStamina()
       {
           if (isReadyToSprint){
               stamina -= staminaRegen * Time.deltaTime;
           }
       }

       public void SlideStamina()
       {
           if (isReadyToSlide && isReadyToSprint){
               stamina -= 100f;
           }
       }

       #endregion

       private void MyInput()
       {
           if (jumpInput && readyToJump && grounded)
           {
               readyToJump = false;
               Jump(GameMetrics.Global.JumpForce);
               Invoke(nameof(ResetJump), GameMetrics.Global.JumpCoolDown);
           }

           else if (jumpInput && readyToJump && nbrJumps > 0 && !exitWallRun && !wallRunning)
           {
               readyToJump = false;
               nbrJumps--;
               Jump(GameMetrics.Global.JumpForce);
               Invoke(nameof(ResetJump), GameMetrics.Global.JumpCoolDown);
           }

           if (crouchInput && grounded)
           {
               transform.localScale = new Vector3(transform.localScale.x, GameMetrics.Global.CrouchYScale, transform.localScale.z);
               rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
           }

           if (crouchInput)
           {
               transform.localScale = new Vector3(transform.localScale.x, GameMetrics.Global.CrouchYScale, transform.localScale.z);
           }
       }

       private void StateHandler()
       {
           if (wallRunning)
           {
               state = MovementState.wallRunning;
               desiredMoveSpeed = GameMetrics.Global.WallRunSpeed;
           }

           if (sliding)
           {
               state = MovementState.sliding;

               if (OnSlope() && rb.linearVelocity.y < 0.1f){
                   desiredMoveSpeed = GameMetrics.Global.SlideSpeed;
               }
               else{
                   desiredMoveSpeed = GameMetrics.Global.SprintSpeed;
               }
           }

           else if (crouchInput)
           {
               //cam.DoFOV(60f);
               state = MovementState.crouching;
               desiredMoveSpeed = GameMetrics.Global.CrouchSpeed;
           }

           else if (runInput && grounded && isReadyToSprint)
           {
               if (flatVel.magnitude > 1){
                   //cam.DoFOV(80f);
                   //CameraShaker.Instance.ShakeOnce(.75f, .30f, .5f, .5f);
                   state = MovementState.sprinting;
                   desiredMoveSpeed = GameMetrics.Global.SprintSpeed;
                   //stamina.DecreaseStamina();
               }
           }

           else if (grounded)
           {
               state = MovementState.walking;
               desiredMoveSpeed = GameMetrics.Global.WalkSpeed;
               if (flatVel.magnitude > 1){
                   //CameraShaker.Instance.ShakeOnce(.50f, .20f, .5f, .25f);
                   //cam.DoFOV(70f);
               }
           }

           else
           {
               state = MovementState.air;
               //CameraShaker.Instance.ShakeOnce(.75f, .30f, .5f, 1f);
               //cam.DoFOV(70f);
           }

           if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
           {
               StopAllCoroutines();
               StartCoroutine(SmoothlyLerpMoveSpeed());
           }
           else
           {
               moveSpeed = desiredMoveSpeed;
           }
           if (vel.magnitude < 1)
           {
               StopAllCoroutines();
           }

           if (flatVel.magnitude > 15)
           {
               //cam.DoFOV(90f);
           }

           lastDesiredMoveSpeed = desiredMoveSpeed;
       }

       private IEnumerator SmoothlyLerpMoveSpeed()
       {
           float time = 0;
           float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
           float startValue = moveSpeed;

           while (time < difference)
           {
               moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

               if (OnSlope())
               {
                   float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                   float slopeAngleIncrease = 1+ (slopeAngle / 90f);
                   time += Time.deltaTime * GameMetrics.Global.SpeedIncreaseMultiplier * GameMetrics.Global.SlopeIncreaseMultiplier * slopeAngleIncrease;
               }
               else
               {
                   time += Time.deltaTime * GameMetrics.Global.SpeedIncreaseMultiplier;
               }

               yield return null;
           }

           moveSpeed = desiredMoveSpeed;
       }

       private void MovePlayer()
       {
           // calculate movement direction
           moveDirection = orientation.forward * moveDirection.x + orientation.right * moveDirection.z;

           // on slope
           if (OnSlope() && !exitSlope)
           {
               rb.AddForce(GetSlopeMoveDirection(moveDirection) * (moveSpeed * 20f), ForceMode.Force);

               if (rb.linearVelocity.y > 0)
                   rb.AddForce(Vector3.down * 80f, ForceMode.Force);
           }

           // on ground
           else if(grounded)
               rb.AddForce(moveDirection.normalized * (moveSpeed * 10f), ForceMode.Force);

           // in air
           else if(!grounded)
               rb.AddForce(moveDirection.normalized * (moveSpeed * 10f * GameMetrics.Global.AirMultiplier), ForceMode.Force);

           // turn gravity off while on slope
           rb.useGravity = !OnSlope();
       }

       private void SpeedControl()
       {
           // limiting speed on slope
           if (OnSlope() && !exitSlope)
           {
               if (rb.linearVelocity.magnitude > moveSpeed)
                   rb.linearVelocity = rb.linearVelocity.normalized * (moveSpeed + slidingForce);
           }

           // limiting speed on ground or in air
           else
           {
               flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

               // limit velocity if needed
               if (flatVel.magnitude > moveSpeed)
               {
                   Vector3 limitedVel = flatVel.normalized * (moveSpeed + slidingForce);
                   rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
               }
           }
       }

       public void Jump(float addJumpForce)
       {
           exitSlope = true;

           rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

           rb.AddForce(transform.up * addJumpForce, ForceMode.Impulse);
       }

       public void ResetJump()
       {
           readyToJump = true;

           exitSlope = false;
       }

       public bool OnSlope()
       {
           if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, GameMetrics.Global.PlayerHeight * 0.5f + 0.3f))
           {
               float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
               return angle < GameMetrics.Global.MaxSlopeAngle && angle != 0;
           }

           return false;
       }

       public Vector3 GetSlopeMoveDirection(Vector3 direction)
       {
           return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
       }

       private void DoVel()
       {
           flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
           vel = rb.linearVelocity;
       }
       */

    }
}