using DeadLink.Entities;
using DeadLink.Weapons.Data;
using RogueLike;
using UnityEngine;

namespace DeadLink.Weapons.WeaponsClass
{
    public class AutomaticRifleWeapon : Weapon
    {
        public override WeaponData WeaponData => automaticWeaponData;
        [SerializeField] private AutomaticWeaponData automaticWeaponData;
        public override int CurrentMunitions { get; protected set; }
        public override float CurrentReloadTime { get; protected set; }

        public override void Fire(Entity entity, Vector3 direction)
        {
            base.Fire(entity, direction);
            AudioManager.Global.PlayOneShot(GameMetrics.Global.FMOD_PlayerAutomaticShoot, entity.transform.position);
        }
    }
}