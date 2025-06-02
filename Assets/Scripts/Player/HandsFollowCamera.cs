using UnityEngine;

namespace DeadLink.Player
{
    public class HandsFollowCamera : MonoBehaviour
    {
        [Header("Follow Rotation")]
        [SerializeField] private Transform cameraRotations;
        
        [Header("Follow Position")]
        [SerializeField] private Transform headTransform;
        [SerializeField] private float armOffset = -0.25f;

        private void LateUpdate()
        {
            MoveArms();
        }

        private void MoveArms()
        {
            Vector3 pos = headTransform.position;
            pos -= -cameraRotations.up * armOffset;

            transform.position = Vector3.Lerp(transform.position, pos, 50f * Time.deltaTime);
        }
    }
}