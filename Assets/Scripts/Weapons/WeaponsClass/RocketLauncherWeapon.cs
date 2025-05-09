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

        public override bool Fire(Entity entity, Vector3 direction)
        {
            if (base.Fire(entity, direction))
            {
                //AudioManager.Global.PlayOneShot(GameMetrics.Global.FMOD_PlayerAutomaticShoot, entity.transform.position);
                return true;
            }

            return false;
        }
    }
}