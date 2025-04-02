using UnityEngine;
using UnityEngine.InputSystem;

namespace RogueLike.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        private Vector2 targetCamVelocity;
        private Vector2 camRotation;
        
        [SerializeField] private float speed;
        [SerializeField, Range(0,1)] private float xModifier = 1;
        [SerializeField, Range(0,1)] private float yModifier = 1;
        [SerializeField] private int yRange = 70;
        
        private void Update()
        {
            camRotation.x += targetCamVelocity.x * speed;
            camRotation.y += targetCamVelocity.y * speed;
            camRotation.x = Mathf.Clamp(camRotation.x, -yRange, yRange);
            
            //Debug.Log(camRotation);
            
            var xQuat = Quaternion.AngleAxis(camRotation.y, Vector3.up);
            var yQuat = Quaternion.AngleAxis(-camRotation.x, Vector3.left);

            transform.localRotation = xQuat * yQuat;
        }

        public void OnLookX(InputAction.CallbackContext context)
        {
            targetCamVelocity.y = context.ReadValue<float>() * xModifier;
        }

        public void OnLookY(InputAction.CallbackContext context)
        {
            targetCamVelocity.x = context.ReadValue<float>() * yModifier;
        }
    }
}