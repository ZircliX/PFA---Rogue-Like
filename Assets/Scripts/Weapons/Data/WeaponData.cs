using UnityEngine;
using UnityEngine.VFX;

namespace DeadLink.Weapons.Data
{
    public abstract class WeaponData : ScriptableObject
    {
        [field: Header("VFX")]
        [field : SerializeField] public VisualEffect ShootVFX { get; private set; }
        [field : SerializeField] public WeaponRecoilSettings WeaponRecoilSettings { get; private set; }
        
        public abstract float ShootRate { get; }
        public abstract int MaxAmmunition { get; }
        public abstract float ReloadTime { get; }
        
        [field: Header("Prefab")]
        [field : SerializeField] public Weapon WeaponPrefab { get; private set; }
    }
}