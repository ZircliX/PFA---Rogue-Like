using RogueLike.Player;
using UnityEngine;

namespace DeadLink.Level
{
    [RequireComponent(typeof(Collider))]
    public class Teleporter : MonoBehaviour
    {
        [SerializeField] private Transform teleportDestination;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.transform.position = teleportDestination.position;
                PlayerMovement pm = other.GetComponent<PlayerMovement>();
            }
        }
    }
}