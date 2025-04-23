using UnityEngine;

namespace DeadLink.Misc
{
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField] private bool invert;
        private Camera cam;
        
        private void Awake()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            Vector3 targetTransform = cam.transform.position;
            
            if (invert)
            {
                Vector3 oppositeDirection = transform.position - cam.transform.position;
                targetTransform = transform.position + oppositeDirection;
            }
            
            transform.LookAt(targetTransform);
        }
    }
}