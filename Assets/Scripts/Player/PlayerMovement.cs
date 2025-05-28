using System.Collections;
using DeadLink.Cameras;
using DeadLink.Entities.Movement;
using DeadLink.Level.CheckPoint;
using DeadLink.Menus;
using DeadLink.Misc;
using DeadLink.SceneManagement;
using LTX.ChanneledProperties;
using RogueLike.Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace RogueLike.Player
{
    public class PlayerMovement : EntityMovement
    {
        #region References

        [field: Header("References")]
        [field: SerializeField] public Camera Camera { get; private set; }
        
        #endregion

        private Coroutine voidEnter;
        
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

        public override void OnVoidDetection()
        {
            if (voidEnter != null) return;
            voidEnter = StartCoroutine(OnVoidEnter());
        }

        private IEnumerator OnVoidEnter()
        {
            //LevelManager.Instance.SaveCurrentLevelScenario();
            
            FadeUI.Instance.FadeIn(0.35f);
            yield return new WaitForSeconds(0.35f);
            
            //Reload Scenario + Teleport to CheckPoint
            LevelManager.Instance.ReloadFromLastScenario();
            if (LevelManager.Instance.PlayerController.PlayerEntity.EmptyHealthBar())
            {
                IMenu menu = MenuManager.Instance.GetMenu(GameMetrics.Global.DieMenu);
                MenuManager.Instance.OpenMenu(menu);
            }
            yield return new WaitForSeconds(0.5f);
            
            FadeUI.Instance.FadeOut(0.35f);

            voidEnter = null;
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
                jumpInputPressed = true;
            }
            else if (context.canceled)
            {
                jumpInputPressed = false;
            }
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

        public void ReadInputDash(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
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