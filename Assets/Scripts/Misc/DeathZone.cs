using DeadLink.Entities.Movement;
using UnityEngine;

namespace DeadLink.Misc
{
    public class DeathZone : MonoBehaviour
    {
        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out EntityMovement entityMovement))
            {
                entityMovement.OnVoidDetection();
            }
        }
    }
}