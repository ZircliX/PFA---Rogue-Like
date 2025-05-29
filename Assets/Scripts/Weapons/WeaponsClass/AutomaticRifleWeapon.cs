using System.Collections;
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

        public override bool Fire(Entity entity, Vector3 direction, GameObject shouldHit)
        {
            if (base.Fire(entity, direction, shouldHit))
            {
                AudioManager.Global.PlayOneShot(GameMetrics.Global.FMOD_PlayerAutomaticShoot, entity.transform.position);
                return true;
            }

            return false;
        }
        
        public override IEnumerator Reload(Entity entity)
        {
            AudioManager.Global.PlayOneShot(GameMetrics.Global.FMOD_PlayerAutomaticReload, entity.transform.position);
            return base.Reload(entity);
        }
    }
}