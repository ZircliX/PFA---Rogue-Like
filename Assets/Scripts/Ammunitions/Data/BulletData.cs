using DeadLink.Cameras;
using RogueLike.VFX;
using UnityEngine;
using UnityEngine.VFX;

namespace DeadLink.Ammunitions.Data
{
    public abstract class BulletData : ScriptableObject
    {
        [field: Header("Base Stats")]
        [field : SerializeField] public float BulletSpeed { get; private set; }
        [field : SerializeField] public float Damage { get; private set; }
        [field : SerializeField] public float MaxLifeCycle { get; private set; }
        
        [field: Header("VFX")]
        [field : SerializeField] public VfxComponent HitVFX { get; private set; }
        [field : SerializeField] public CameraShakeComposite CameraShake { get; private set; }
        
        [field: Header("Prefab")]
        [field : SerializeField] public Bullet BulletPrefab { get; private set; }
    }
}