using KBCore.Refs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RogueLike.Player
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class PlayerMovement : MonoBehaviour
    {
        private Vector2 inputDirection;
        [SerializeField, Self] private Rigidbody rb;

        private void OnValidate()
        {
            this.ValidateRefs();
        }

        private void Start()
        {
            AudioManager.Instance.PlayOneShot(GameMetrics.Global.test, transform.position);
        }

        private void FixedUpdate()
        {
            Vector3 moveDirection = new Vector3(inputDirection.x, 0, inputDirection.y);
            
            rb.AddForce(moveDirection * GameMetrics.Global.PlayerBaseSpeed, ForceMode.Force);
            
        } 

        public void ReadInputMove(InputAction.CallbackContext context)
        {
            inputDirection = context.ReadValue<Vector2>();
        }
    }
}