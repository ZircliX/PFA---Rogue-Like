using System.Collections;
using DeadLink.Entities;
using DeadLink.Menus;
using DeadLink.Weapons.Data;
using RogueLike;
using RogueLike.Controllers;
using RogueLike.Managers;
using RogueLike.VFX;
using UnityEngine;

namespace DeadLink.Weapons.WeaponsClass
{
    public class LaserWeapon : Weapon
    {
        public override WeaponData WeaponData => laserWeaponData;
        
        [SerializeField] private LaserWeaponData laserWeaponData;
        [SerializeField] private VfxComponent laserVfx;
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

        public override bool Fire(Entity entity, Vector3 direction, GameObject shouldHit)
        {
            if (base.Fire(entity, direction, shouldHit))
            {
                if (CurrentMunitions <= 0)
                {
                    AudioManager.Global.PlayOneShot(GameMetrics.Global.FMOD_PlayerLaserOverHeat, entity.transform.position);
                    StartCoroutine(CooldownLaser(entity));
                }
                else
                {
                    AudioManager.Global.PlayOneShot(GameMetrics.Global.FMOD_PlayerLaserShoot, entity.transform.position);
                    if (laserVfx != null)
                    {
                        laserVfx.PlayVFX(LevelManager.Instance.PlayerController.ArmFlashPosition.position, LevelManager.Instance.PlayerController.delayAfterDestroyFlash);
                    }
                }
                return true;
            }

            return false;
        }
    }
}