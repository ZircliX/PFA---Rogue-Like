using UnityEngine;
using UnityEngine.VFX;

namespace RogueLike.Controllers
{
    public class VFXManager
    {
        public void VFX(VisualEffect vfx, Vector3 position, float delayAfterDestroyVfx)
        {
            VisualEffect spawnedVFX = Object.Instantiate(vfx, position, Quaternion.identity);
            
            if (spawnedVFX != null)
            {
                spawnedVFX.Play();
                Object.Destroy(spawnedVFX.gameObject, delayAfterDestroyVfx);
            }
        }
    }
}