using System.Collections;
using DeadLink.Entities;
using DeadLink.Weapons.Data;
using DeadLink.Weapons.Data.Enemies;
using RogueLike;
using UnityEngine;

namespace DeadLink.Weapons.WeaponsClass.Enemies
{
    public class SpiderWeapon : Weapon
    {
        public override WeaponData WeaponData => weaponData;
        [SerializeField] private SpiderWeaponData weaponData;
        public override int CurrentMunitions { get; protected set; }
        public override float CurrentReloadTime { get; protected set; }
        
        public override bool Fire(Entity entity, Vector3 direction, GameObject shouldHit)
        {
            if (base.Fire(entity, direction, shouldHit))
            {
                
                return true;
            }

            return false;
        }
        
        public override IEnumerator Reload(Entity entity)
        {
            return base.Reload(entity);
        }
    }
}