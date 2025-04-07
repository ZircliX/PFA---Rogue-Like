using DeadLink.Ammunitions;
using DeadLink.Ammunitions.Data;
using DeadLink.Entities;
using DeadLink.Weapons.Data;
using UnityEngine;

namespace DeadLink.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [field : SerializeField] public WeaponData WeaponData { get; private set; }
        [field : SerializeField] public BulletData BulletData { get; private set; }


        public virtual void Fire(Entity entity, Vector3 direction)
        {
            Debug.Log($"Instantiating bullet {BulletData.name} from {entity.name}");
            
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