using DeadLink.Entities;
using DeadLink.Weapons.Data;
using UnityEngine;

namespace DeadLink.Weapons.WeaponsClass
{
    public class RocketLauncherWeapon : Weapon
    {
        public override WeaponData WeaponData => rocketWeaponData;
        [SerializeField] private RocketWeaponData rocketWeaponData;
        public override int CurrentMunitions { get; protected set; }
        public override float CurrentReloadTime { get; protected set; }

        public override void Fire(Entity entity, Vector3 direction)
        {
            base.Fire(entity, direction);
        }
    }
}