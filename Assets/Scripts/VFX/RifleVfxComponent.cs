using RogueLike.Managers;
using UnityEngine;

namespace DeadLink.VFX
{
    public class RifleVfxComponent : MonoBehaviour
    {
        private void Update()
        {
            if (LevelManager.Instance.PlayerController.ArmFlashPosition != null)
            {
                transform.position = LevelManager.Instance.PlayerController.ArmFlashPosition.position;
                transform.rotation = LevelManager.Instance.PlayerController.ArmFlashPosition.rotation;
            }
        }
    }
}