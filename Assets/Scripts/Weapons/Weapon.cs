using System.Collections;
using DeadLink.Ammunitions;
using DeadLink.Ammunitions.Data;
using DeadLink.Cameras;
using DeadLink.Entities;
using DeadLink.Menus;
using DeadLink.Weapons.Data;
using LTX.ChanneledProperties;
using RogueLike.Managers;
using UnityEngine;

namespace DeadLink.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        public abstract WeaponData WeaponData { get; }
        [field : SerializeField] public BulletData BulletData { get; private set; }

        public abstract int CurrentMunitions { get; protected set; }
        public abstract float CurrentReloadTime { get; protected set; }
        protected bool isShooting;

        private void OnEnable()
        {
            CameraController.Instance.CameraShakeProperty.AddPriority(this, PriorityTags.Default);
        }

        private void OnDisable()
        {
            if (CameraController.HasInstance)
                CameraController.Instance.CameraShakeProperty.RemovePriority(this);
        }

        protected virtual void Awake()
        {
            SetMaxBullets();
            CurrentReloadTime = WeaponData.ReloadTime;
        }

        public virtual void OnUpdate(Entity entity)
        {
            
        }

        protected void SetMaxBullets()
        {
            CurrentMunitions = WeaponData.MaxAmmunition;
        }
        
        public virtual bool Fire(Entity entity, Vector3 direction, GameObject shouldHit)
        {
            //Debug.Log($"entity : {entity.name}, direction : {direction}, shouldHit : {shouldHit?.name}");
            if (!MenuManager.Instance.TryGetCurrentMenu(out IMenu menu) || menu.MenuType != MenuType.HUD) return false;
            
            if (!entity.ContinuousFire)
            {
                if (CurrentMunitions <= 0)
                {
                    //play sound
                    StartCoroutine(Reload(entity));
                    return false;
                }
            }
            
            //Debug.Log("Spawn bullet");
            Bullet bullet = Instantiate(
                BulletData.BulletPrefab, 
                entity.BulletSpawnPoint.position, 
                BulletData.BulletPrefab.transform.rotation);

            bullet.OnBulletHit += BulletHit;
            bullet.OnBulletDestroy += BulletDestroy;
            
            //Debug.Log("bullet shoot");
            bullet.Shoot(entity, direction, shouldHit);

            if (entity.CompareTag("Player"))
            {
                WeaponRecoilSettings recoilData = WeaponData.WeaponRecoilSettings;
                CameraController.Instance.RecoilFire(recoilData.GetRecoil(), recoilData.Snappiness, recoilData.ReturnSpeed);
            }

            if (!entity.ContinuousFire)
            {
                CurrentMunitions--;
            }

            if (entity.CompareTag("Player"))
            {
                MenuManager.Instance.HUDMenu.UpdateAmmunitions(CurrentMunitions, WeaponData.MaxAmmunition);
                MenuManager.Instance.HUDMenu.SetCrosshairOffset();
            }

            return true;
        }
        
        public void SetShootingState(bool canShoot)
        {
            isShooting = canShoot;
        }

        public virtual IEnumerator Reload(Entity entity)
        {
            CurrentReloadTime = 0;
            int previousMunitions = CurrentMunitions;

            while (CurrentReloadTime < WeaponData.ReloadTime)
            {
                CurrentReloadTime += Time.deltaTime;
                float reloadProgress = CurrentReloadTime / WeaponData.ReloadTime;
                CurrentMunitions = Mathf.FloorToInt(Mathf.Lerp(previousMunitions, WeaponData.MaxAmmunition, reloadProgress));

                if (entity.CurrentWeapon == this && entity.CompareTag("Player"))
                    MenuManager.Instance.HUDMenu.UpdateAmmunitions(CurrentMunitions, WeaponData.MaxAmmunition);
                yield return null;
            }

            CurrentReloadTime = WeaponData.ReloadTime;
            
            SetMaxBullets();
            if (entity.CurrentWeapon == this && entity.CompareTag("Player"))
                MenuManager.Instance.HUDMenu.UpdateAmmunitions(CurrentMunitions, WeaponData.MaxAmmunition);
        }

        protected virtual void BulletHit(Bullet bullet)
        {
            CameraController.Instance.CameraShakeProperty.Write(this, bullet.BulletData.CameraShake);
            //Debug.Log($"Called Camera Shake from {name} with {bullet.name}");
        }

        protected virtual void BulletDestroy(Bullet bullet)
        {
            bullet.OnBulletHit -= BulletHit;
            bullet.OnBulletDestroy -= BulletDestroy;
        }
    }
}