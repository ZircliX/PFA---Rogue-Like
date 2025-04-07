using UnityEngine;

namespace DeadLink.Weapons.Data
{
    public abstract class ProjectileWeaponData : WeaponData
    {
        [field: Header("Stats")]
        [field : SerializeField] public float FireRate { get; private set; }
        [field : SerializeField] public float ReloadDuration { get; private set; }
        [field : SerializeField] public int MagCapacity { get; private set; }
    }
}