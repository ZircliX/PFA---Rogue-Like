using UnityEngine;
using UnityEngine.VFX;

namespace RogueLike.Controllers
{
    public static class VFXExtensions
    {
        public static void PlayVFX(this VisualEffect vfx, Vector3 position, float delayAfterDestroyVfx)
        {
            if (vfx == null) return;
            
            VisualEffect spawnedVFX = Object.Instantiate(vfx, position, Quaternion.identity);
            
            if (spawnedVFX != null)
            {
                spawnedVFX.Play();
                Object.Destroy(spawnedVFX.gameObject, delayAfterDestroyVfx);
            }
        }
    }
}