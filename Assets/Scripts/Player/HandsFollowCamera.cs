using UnityEngine;

namespace DeadLink.Player
{
    public class HandsFollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform handsRoot;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private bool followPitch = true;

        private void LateUpdate()
        {
            Vector3 euler = cameraTransform.eulerAngles;
        
            handsRoot.rotation = Quaternion.Lerp(
                handsRoot.rotation,
                Quaternion.Euler(followPitch ? euler.x : 0f,
                    euler.y,
                    0f),
                0.25f
            );
        }
    }
}