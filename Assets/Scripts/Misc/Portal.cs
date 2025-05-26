using RogueLike.Controllers;
using RogueLike.Entities;
using UnityEngine;

namespace DeadLink.Misc
{
    public class Portal : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerEntity entity))
            {
                GameController.SceneController.GoToNextLevel(gameObject.scene.buildIndex);
            }
        }
    }
}