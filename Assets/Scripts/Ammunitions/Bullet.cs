using DeadLink.Ammunitions.Data;
using DeadLink.Entities;
using KBCore.Refs;
using UnityEngine;

namespace DeadLink.Ammunitions
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Bullet : MonoBehaviour
    {
        [field: SerializeField] public BulletData BulletData { get; private set; }
        [SerializeField, Self] private Rigidbody rb;
        private float damage;

        private void OnValidate() => this.ValidateRefs();
        
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Entity entity))
            {
                Hit(entity);
            }
        }

        public void Shoot(float entityStrength, Vector3 direction)
        {
            damage = BulletData.Damage * entityStrength;
            rb.AddForce(direction * BulletData.BulletSpeed, ForceMode.Impulse);
        }

        protected virtual void Hit(Entity entity)
        {
            entity.TakeDamage(damage);
        }
    }
}