using DeadLink.Cameras;
using DeadLink.Entities.Movement;
using DeadLink.Extensions;
using DeadLink.Menus;
using DeadLink.PowerUpSystem;
using LTX.ChanneledProperties;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RogueLike.Player
{
    public class PlayerMovement : EntityMovement
    {
        #region Power ups

        private int remainingJump = 1;
        private int remainingDash = 1;
        public void AddBonusJump(int value) => remainingJump += value;
        public void AddBonusDash(int value) => remainingDash += value;
        
        public void StartCooldownCoroutine(CooldownPowerUp cooldownPowerUp)
        {
            StartCoroutine(cooldownPowerUp.Cooldown());
        }
        
        public void ActiveQuickFall()
        {
            canChangeGravityScale = false;
            gravityScale = 10f;
        }
        public void DesactiveQuickFall()
        {
            gravityScale = 1f;
            canChangeGravityScale = true;
        }
        #endregion

        #region References

        [field: Header("References")]
        [field: SerializeField] public Camera Camera { get; private set; }
        
        #endregion
        
        protected override void Awake()
        {
            base.Awake();
            CameraController.Instance.CameraEffectProperty.AddPriority(stateChannelKey, PriorityTags.High);
        }

        protected override void FixedUpdate()
        {
            if (!MenuManager.Instance.TryGetCurrentMenu(out IMenu menu) || menu.MenuType != MenuType.HUD) return;
            base.FixedUpdate();
            CameraController.Instance.CameraEffectProperty.Write(stateChannelKey, movementStates[currentStateIndex].GetCameraEffects(this, Time.deltaTime));
        }

        protected override void HandleVoidDetection()
        {
            base.HandleVoidDetection();
            if (Position.y < - Mathf.Abs(maxYPosition))
            {
                this.TeleportPlayer(lastSafePosition, 1, 2);
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
                jumpInputPressed = true;
            }
            else if (context.canceled)
            {
                //jumpInput = 0;
                jumpInputPressed = false;
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
                DashInput = true;
            }
            else if (context.canceled)
            {
                DashInput = false;
            }
        }

        #endregion

    }
}