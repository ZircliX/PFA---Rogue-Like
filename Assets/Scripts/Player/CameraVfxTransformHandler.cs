using LTX.Singletons;
using RogueLike.VFX;
using UnityEngine;

namespace DeadLink.Player
{
    public class CameraVfxTransformHandler : MonoSingleton<CameraVfxTransformHandler>
    {
        [field : SerializeField] public Transform DashPosition { get; private set; }
        [field : SerializeField] public VfxComponent DashComponent { get; private set; }
        
        [field : SerializeField] public Transform OverdrivePosition { get; private set; }
        [field : SerializeField] public VfxComponent OverdriveComponent { get; private set; }

        [field : SerializeField] public Transform SlowMotionPosition { get; private set; }
        [field : SerializeField] public VfxComponent SlowMotionComponent { get; private set; }

        [field : SerializeField] public Transform RewindPosition { get; private set; }
        [field : SerializeField] public VfxComponent RewindComponent { get; private set; }

        [field : SerializeField] public Transform PermaShotPosition { get; private set; }
        [field : SerializeField] public VfxComponent PermaShotComponent { get; private set; }

    }
}