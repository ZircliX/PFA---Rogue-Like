using UnityEngine;

namespace DeadLink.Weapons.Data
{
    public abstract class ProjectileWeaponData : WeaponData
    {
        [field: Header("Shoot Stats")]
        [SerializeField] private float shootRate;
        public override float ShootRate => shootRate;
        [SerializeField] private int maxAmmunition;
        public override int MaxAmmunition => maxAmmunition;

        [SerializeField] private float reloadDuration;
        public override float ReloadTime => reloadDuration;
    }
}