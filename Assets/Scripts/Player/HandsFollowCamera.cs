using UnityEngine;

namespace DeadLink.Player
{
    public class HandsFollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Transform headTransform;
        [SerializeField] private float armOffset = -0.25f;
        [SerializeField] private bool followPitch = true;

        private void LateUpdate()
        {
            MoveArms();
            RotateArms();
        }

        private void MoveArms()
        {
            Vector3 pos = headTransform.position;
            pos -= -cameraTransform.up * armOffset;

            transform.position = Vector3.Lerp(transform.position, pos, 40f * Time.deltaTime);
        }

        private void RotateArms()
        {
            Vector3 euler = cameraTransform.eulerAngles;
        
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.Euler(followPitch ? euler.x : 0f,
                    euler.y,
                    0f),
                40f * Time.deltaTime
            );
        }
    }
}