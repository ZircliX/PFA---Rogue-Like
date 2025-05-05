using System;
using DeadLink.Ammunitions.Data;
using DeadLink.Entities;
using KBCore.Refs;
using RogueLike.Controllers;
using UnityEngine;
using UnityEngine.VFX;

namespace DeadLink.Ammunitions
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Bullet : MonoBehaviour
    {
        public abstract BulletData BulletData { get; }
        [SerializeField, Self] private Rigidbody rb;
        public abstract string AuthorTag { get; set; }
        
        public event Action<Bullet> OnBulletHit;
        public event Action<Bullet> OnBulletDestroy;
        
        protected float damage;
        protected Vector3 lastPosition;
        protected float currentLifeCycle;

        private void OnValidate() => this.ValidateRefs();
        
        protected virtual void FixedUpdate()
        {
            Vector3 currentPosition = transform.position;
            Vector3 direction = currentPosition - lastPosition;
            float distance = direction.magnitude;
            
            if (distance > 0)
            {
                Ray ray = new Ray(lastPosition, direction.normalized);
                
                if (Physics.Raycast(ray, out RaycastHit hit, distance, ~0, QueryTriggerInteraction.Ignore))
                {
                    if (hit.collider.TryGetComponent(out Entity entity))
                    {
                        //Debug.Log($"Hit Entity {entity.name}");
                        ApplyDamage(entity);
                        HitObject(hit);
                    }
                    else if (hit.collider.gameObject.GetInstanceID() != gameObject.GetInstanceID())
                    {
                        //Debug.Log($"Hit object {hit.collider.name}");
                        HitObject(hit);
                    }
                    
                    else if (hit.collider.gameObject.GetInstanceID() == gameObject.GetInstanceID())
                    {
                        currentLifeCycle += Time.deltaTime;
                        if (currentLifeCycle >= BulletData.MaxLifeCycle)
                        {
                            DestroyBullet();
                        }
                    }
                }
            }
            
            lastPosition = currentPosition;
        }

        public void Shoot(float entityStrength, Vector3 direction)
        {
            damage = BulletData.Damage * entityStrength;
            lastPosition = transform.position;
            rb.AddForce(direction * BulletData.BulletSpeed, ForceMode.Impulse);
        }

        protected virtual void ApplyDamage(params Entity[] entities)
        {
            entities ??= Array.Empty<Entity>();
            for (int i = 0; i < entities.Length; i++)
            {
                Entity entity = entities[i];
                if (entity.CompareTag(AuthorTag)) continue;
                entity.TakeDamage(damage);
            }
        }

        protected virtual void HitObject(RaycastHit hit)
        {
            //Debug.Log($"Author Result {hit.collider.CompareTag(AuthorTag)}, Hit Result {hit.collider.gameObject.name}");
            if (hit.collider.CompareTag(AuthorTag)) return;
            
            //+ hit.normal * 0.5f
            BulletData.HitVFX.PlayVFX(hit.point, 2);
            
            OnBulletHit?.Invoke(this);
            DestroyBullet();
        }

        protected virtual void DestroyBullet()
        {
            OnBulletDestroy?.Invoke(this);
            Destroy(gameObject);
        }
    }
}