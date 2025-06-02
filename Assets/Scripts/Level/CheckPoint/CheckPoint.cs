using UnityEngine;

namespace DeadLink.Level.CheckPoint
{
    public class CheckPoint : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                CheckPointManager.Instance.SetCheckPoint(this);
            }
        }
    }
}