using UnityEngine;
using UnityEngine.VFX;

namespace RogueLike.VFX
{
    public class VfxComponent : MonoBehaviour
    {
        [field: SerializeField] public ParticleSystem[] vfxPsArray { get; private set; }
        [field: SerializeField] public VisualEffect[] vfxVeArray { get; private set; }

    }
}