using System;
using DeadLink.Ammunitions;
using DeadLink.Ammunitions.Data;
using DeadLink.Cameras;
using DeadLink.Entities;
using DeadLink.Menus;
using DeadLink.Menus.Implementation;
using DeadLink.Weapons.Data;
using LTX.ChanneledProperties;
using RogueLike.Managers;
using UnityEngine;

namespace DeadLink.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [field : SerializeField] public WeaponData WeaponData { get; private set; }
        [field : SerializeField] public BulletData BulletData { get; private set; }

        public int CurrentMunitions { get; private set; }

        private void OnEnable()
        {
            CameraController.Instance.CameraShakeProperty.AddPriority(this, PriorityTags.Default);
        }

        private void OnDisable()
        {
            CameraController.Instance.CameraShakeProperty.RemovePriority(this);
        }

        private void Awake()
        {
            SetMaxBullets();
            gameObject.SetActive(false);
        }

        private void SetMaxBullets()
        {
            CurrentMunitions = WeaponData.MaxAmmunition;
        }
        
        public virtual void Fire(Entity entity, Vector3 direction)
        {
            if (CurrentMunitions <= 0)
            {
                //play sound
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

        public virtual void Reload()
        {
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