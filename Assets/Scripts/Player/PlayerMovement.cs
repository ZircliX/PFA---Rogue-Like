using UnityEngine;

namespace RogueLike.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private void Start()
        {
            AudioManager.Instance.PlayOneShot(GameMetrics.Global.test, transform.position);
        }
    }
}