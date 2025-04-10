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

        private int currentMunitions;
        
        private void Awake()
        {
            CameraController.Instance.CameraShakeProperty.AddPriority(this, PriorityTags.Small);
        }

        private void OnDestroy()
        {
            if (CameraController.HasInstance)
            {
                CameraController.Instance.CameraShakeProperty.RemovePriority(this);
            }
        }

        public virtual void Fire(Entity entity, Vector3 direction)
        {
            //Debug.Log($"Instantiating bullet {BulletData.name} from {entity.name}");
            CameraController.Instance.CameraShakeProperty.Write(this, WeaponData.CameraShake);
            
            Bullet bullet = Instantiate(
                BulletData.BulletPrefab, 
                entity.BulletSpawnPoint.position, 
                BulletData.BulletPrefab.transform.rotation);

            bullet.OnBulletHit += BulletHit;
            bullet.OnBulletDestroy += BulletDestroy;
            
            bullet.Shoot(entity.Strength, entity.BulletSpawnPoint.transform.forward);
        }

        public virtual void Reload()
        {
            
        }

        protected virtual void BulletHit(Bullet bullet)
        {
            CameraController.Instance.CameraShakeProperty.Write(this, bullet.BulletData.CameraShake);
        }

        protected virtual void BulletDestroy(Bullet bullet)
        {
            bullet.OnBulletHit -= BulletHit;
            bullet.OnBulletDestroy -= BulletDestroy;
        }
    }
}