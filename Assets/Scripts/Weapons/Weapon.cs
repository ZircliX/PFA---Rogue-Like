using System;
using DeadLink.Ammunitions;
using DeadLink.Ammunitions.Data;
using DeadLink.Cameras;
using DeadLink.Entities;
using DeadLink.Weapons.Data;
using LTX.ChanneledProperties;
using UnityEngine;

namespace DeadLink.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [field : SerializeField] public WeaponData WeaponData { get; private set; }
        [field : SerializeField] public BulletData BulletData { get; private set; }
        
        private void Awake()
        {
            CameraController.Instance.CameraShakeProperty.AddPriority(this, PriorityTags.Small);
        }

        private void OnDestroy()
        {
            CameraController.Instance.CameraShakeProperty.RemovePriority(this);
        }

        public virtual void Fire(Entity entity, Vector3 direction)
        {
            //Debug.Log($"Instantiating bullet {BulletData.name} from {entity.name}");
            CameraController.Instance.CameraShakeProperty.Write(this, WeaponData.CameraShake);
            
            Bullet bullet = Instantiate(
                BulletData.BulletPrefab, 
                entity.BulletSpawnPoint.position, 
                BulletData.BulletPrefab.transform.rotation);
            
            bullet.Shoot(entity.Strength, entity.BulletSpawnPoint.transform.forward);
        }

        public virtual void Reload()
        {
            
        }
    }
}