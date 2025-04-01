using UnityEngine;
using UnityEngine.InputSystem;

namespace RogueLike.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        private Vector2 targetCamVelocity;
        
        [SerializeField] private float speed;
        [SerializeField, Range(0,1)] private float xModifier = 1;
        [SerializeField, Range(0,1)] private float yModifier = 1;
        [SerializeField] private int minY = -80;
        [SerializeField] private int maxY = 80;
        [SerializeField] private Transform head;
        
        private void Update()
        {
            float x = head.eulerAngles.x + targetCamVelocity.x * speed;
            if (x >= 180)
                x -= 360;
            
            x = Mathf.Clamp(x, minY, maxY);
            float y = head.eulerAngles.y + targetCamVelocity.y * speed;
            head.eulerAngles = new Vector3(x, y);
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