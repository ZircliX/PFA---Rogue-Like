using System.Collections;
using DeadLink.Entities;
using DeadLink.Weapons.Data;
using DeadLink.Weapons.Data.Enemies;
using RogueLike;
using UnityEngine;

namespace DeadLink.Weapons.WeaponsClass.Enemies
{
    public class SlashWeapon : Weapon
    {
        public override WeaponData WeaponData => weaponData;
        [SerializeField] private SlashWeaponData weaponData;
        public override int CurrentMunitions { get; protected set; }
        public override float CurrentReloadTime { get; protected set; }
        
        public override bool Fire(Entity entity, Vector3 direction, GameObject shouldHit)
        {
            Debug.Log("SlashWeapon Fire");
            if (base.Fire(entity, direction, shouldHit))
            {
                Debug.Log("SlashWeapon Call audio");
                AudioManager.Global.PlayOneShot(GameMetrics.Global.FMOD_EnemyAttack, entity.transform.position);
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