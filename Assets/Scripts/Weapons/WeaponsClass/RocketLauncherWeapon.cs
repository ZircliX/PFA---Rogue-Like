using System.Collections;
using DeadLink.Entities;
using DeadLink.Weapons.Data;
using RogueLike;
using UnityEngine;

namespace DeadLink.Weapons.WeaponsClass
{
    public class RocketLauncherWeapon : Weapon
    {
        public override WeaponData WeaponData => rocketWeaponData;
        [SerializeField] private RocketWeaponData rocketWeaponData;
        public override int CurrentMunitions { get; protected set; }
        public override float CurrentReloadTime { get; protected set; }

        public override bool Fire(Entity entity, Vector3 direction, GameObject shouldHit)
        {
            if (base.Fire(entity, direction, shouldHit))
            {
                AudioManager.Global.PlayOneShot(GameMetrics.Global.FMOD_PlayerRocketShoot, entity.transform.position);
                return true;
            }

            return false;
        }

        public override IEnumerator Reload(Entity entity)
        {
            AudioManager.Global.PlayOneShot(GameMetrics.Global.FMOD_PlayerRocketReload, entity.transform.position);
            return base.Reload(entity);
        }
    }
}