using RogueLike.VFX;
using UnityEngine;

namespace RogueLike.Controllers
{
    public static class VFXExtensions
    {
        
        public static void PlayVFX(this VfxComponent effects, Vector3 position, float delayAfterDestroyVfx)
        {
            if (effects == null) return;
            
            VfxComponent vfxInstance = Object.Instantiate(effects, position, Quaternion.identity);
            
            for (int i = 0; i < vfxInstance.vfxPsArray.Length; i++)
            {
                effects.vfxPsArray[i].Play();
            }
            
            for (int i = 0; i < vfxInstance.vfxVeArray.Length; i++)
            {
                effects.vfxVeArray[i].Play();
            }
            
            Object.Destroy(vfxInstance.gameObject, delayAfterDestroyVfx);

        }

        public static void PlayVFXCamera(this VfxComponent effects, float delayAfterDestroyVfx, Transform anchorCameraPosition)
        {
            if (effects == null) return;
            
            VfxComponent vfxInstance = Object.Instantiate(effects, anchorCameraPosition.position, Quaternion.identity);
            vfxInstance.Initialize(anchorCameraPosition);
            
            for (int i = 0; i < vfxInstance.vfxPsArray.Length; i++)
            {
                effects.vfxPsArray[i].Play();
            }
            
            for (int i = 0; i < vfxInstance.vfxVeArray.Length; i++)
            {
                effects.vfxVeArray[i].Play();
            }
            
            Object.Destroy(vfxInstance.gameObject, delayAfterDestroyVfx);
        }
    }
}