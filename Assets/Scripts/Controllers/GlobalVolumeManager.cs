using LTX.Singletons;
using UnityEngine;
using UnityEngine.Rendering;

namespace RogueLike.Controllers
{
    public class GlobalVolumeManager : MonoSingleton<GlobalVolumeManager>
    {
        [field: SerializeField] public Volume globalVolume;
        
        
    }
}