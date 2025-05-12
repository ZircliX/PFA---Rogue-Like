using System.Collections;
using DeadLink.Entities;
using DeadLink.Menus;
using DeadLink.Weapons.Data;
using RogueLike.Managers;
using UnityEngine;

namespace DeadLink.Weapons.WeaponsClass
{
    public class LaserWeapon : Weapon
    {
        public override WeaponData WeaponData => laserWeaponData;
        [SerializeField] private LaserWeaponData laserWeaponData;
        public override int CurrentMunitions { get; protected set; }
        public override float CurrentReloadTime { get; protected set; }

        private float currentLaserGainTime;

        public override IEnumerator Reload(Entity entity)
        {
            yield return null;
        }

        /// <summary>
        /// Called when the laser is overheated.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private IEnumerator CooldownLaser(Entity entity)
        {
            CurrentReloadTime = 0;
            
            while (CurrentReloadTime < laserWeaponData.ReloadTime)
            {
                CurrentReloadTime += Time.deltaTime;
                
                CurrentMunitions = Mathf.FloorToInt(CurrentReloadTime / laserWeaponData.ReloadTime * WeaponData.MaxAmmunition);
                
                if (entity.CurrentWeapon == this)
                    MenuManager.Instance.HUDMenu.UpdateAmmunitions(CurrentMunitions, WeaponData.MaxAmmunition);
                
                yield return null;
            }

            CurrentReloadTime = laserWeaponData.ReloadTime;
            
            SetMaxBullets();
            if (entity.CurrentWeapon == this)
                MenuManager.Instance.HUDMenu.UpdateAmmunitions(CurrentMunitions, WeaponData.MaxAmmunition);
        }

        public override void OnUpdate(Entity entity)
        {
            if (CurrentReloadTime < laserWeaponData.ReloadTime) return;
            
            if (!isShooting && CurrentMunitions < WeaponData.MaxAmmunition)
            {
                currentLaserGainTime += Time.deltaTime;
                
                if (currentLaserGainTime >= laserWeaponData.LaserGainRate)
                {
                    currentLaserGainTime = 0;
                    CurrentMunitions++;
                    MenuManager.Instance.HUDMenu.UpdateAmmunitions(CurrentMunitions, WeaponData.MaxAmmunition);
                }
            }
        }

        public override bool Fire(Entity entity, Vector3 direction)
        {
            if (base.Fire(entity, direction))
            {
                if (CurrentMunitions <= 0)
                {
                    StartCoroutine(CooldownLaser(entity));
                }
                //AudioManager.Global.PlayOneShot(GameMetrics.Global.FMOD_PlayerAutomaticShoot, entity.transform.position);
                return true;
            }

            return false;
        }
    }
}