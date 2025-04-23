using System.Collections;
using DeadLink.Ammunitions;
using DeadLink.Ammunitions.Data;
using DeadLink.Cameras;
using DeadLink.Entities;
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
            CameraController.Instance.CameraShakeProperty.RemovePriority(this);
        }

        protected virtual void Awake()
        {
            SetMaxBullets();
            CurrentReloadTime = WeaponData.ReloadTime;
        }

        protected void SetMaxBullets()
        {
            CurrentMunitions = WeaponData.MaxAmmunition;
        }
        
        public virtual void Fire(Entity entity, Vector3 direction)
        {
            if (CurrentReloadTime < WeaponData.ReloadTime) return;
            
            if (CurrentMunitions <= 0)
            {
                //play sound
                StartCoroutine(Reload());
                return;
            }
            
            //Debug.Log($"Instantiating bullet {BulletData.name} from {entity.name}");
            CameraController.Instance.CameraShakeProperty.Write(this, WeaponData.CameraShake);
            
            Bullet bullet = Instantiate(
                BulletData.BulletPrefab, 
                entity.BulletSpawnPoint.position, 
                BulletData.BulletPrefab.transform.rotation);

            bullet.OnBulletHit += BulletHit;
            bullet.OnBulletDestroy += BulletDestroy;
            
            bullet.Shoot(entity.Strength, direction);

            CurrentMunitions--;
            
            LevelManager.Instance.HUDMenuHandler.UpdateAmmunitions(CurrentMunitions, WeaponData.MaxAmmunition);
            LevelManager.Instance.HUDMenuHandler.SetCrosshairOffset();
        }
        
        public void SetShootingState(bool canShoot)
        {
            isShooting = canShoot;
        }

        public virtual IEnumerator Reload()
        {
            CurrentReloadTime = 0;

            while (CurrentReloadTime < WeaponData.ReloadTime)
            {
                CurrentReloadTime += Time.deltaTime;
                
                int previousMunitions = CurrentMunitions;

                float reloadProgress = CurrentReloadTime / WeaponData.ReloadTime;
                reloadProgress = Mathf.Clamp01(reloadProgress);

                CurrentMunitions = Mathf.FloorToInt(Mathf.Lerp(previousMunitions, WeaponData.MaxAmmunition, reloadProgress));
                
                LevelManager.Instance.HUDMenuHandler.UpdateAmmunitions(CurrentMunitions, WeaponData.MaxAmmunition);
                
                yield return null;
            }

            CurrentReloadTime = WeaponData.ReloadTime;
            
            SetMaxBullets();
            LevelManager.Instance.HUDMenuHandler.UpdateAmmunitions(CurrentMunitions, WeaponData.MaxAmmunition);
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