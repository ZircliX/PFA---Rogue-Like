using LTX.Singletons;
using UnityEngine;

namespace RogueLike.Controllers
{
    public class VfxManager : MonoBehaviour
    {
        public void VFX(ParticleSystem vfx, Vector3 position, float delayAfterDestroyVfx)
        {
            ParticleSystem vfxToSpawn = Instantiate(vfx, position, Quaternion.identity);

            if (vfxToSpawn != null)
            {
                vfxToSpawn.Play();
                Destroy(vfxToSpawn.gameObject, delayAfterDestroyVfx);
                
            }
        }
    }
}