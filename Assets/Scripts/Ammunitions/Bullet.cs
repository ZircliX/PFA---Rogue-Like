using System;
using DeadLink.Ammunitions.Data;
using DeadLink.Entities;
using KBCore.Refs;
using RayFire;
using RogueLike.Controllers;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

namespace DeadLink.Ammunitions
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Bullet : MonoBehaviour
    {
        public abstract BulletData BulletData { get; }
        [SerializeField, Self] private Rigidbody rb;
        public Entity Author { get; protected set; }
        
        public event Action<Bullet> OnBulletHit;
        public event Action<Bullet> OnBulletDestroy;
        
        protected float damage;
        protected Vector3 lastPosition;
        protected float currentLifeCycle;
        protected GameObject shouldHit;

        private void OnValidate() => this.ValidateRefs();
        
        protected virtual void FixedUpdate()
        {
            if (shouldHit)
            {
                if (shouldHit.TryGetComponent(out Entity entity))
                {
                    ApplyDamage(entity);
                    HitObject(entity.gameObject);
                }
                else
                {
                    HitObject(shouldHit);
                }
            }
            
            Vector3 currentPosition = rb.position;
            Vector3 direction = currentPosition - lastPosition;
            float distance = direction.magnitude;
            
            if (distance > 0)
            {
                Ray ray = new Ray(lastPosition, direction.normalized);
                
                if (Physics.Raycast(ray, out RaycastHit hit, distance, ~0, QueryTriggerInteraction.Ignore))
                {
                    /*
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
                    */
                    
                    if (hit.collider.gameObject.GetInstanceID() == gameObject.GetInstanceID())
                    {
                        currentLifeCycle += Time.deltaTime;
                        if (currentLifeCycle >= BulletData.MaxLifeCycle)
                        {
                            DestroyBullet();
                        }
                    }
                    else
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

        public void Shoot(Entity entity, Vector3 direction, GameObject shouldHit)
        {
            this.shouldHit = shouldHit;
            Author = entity;
            damage = BulletData.Damage * entity.Strength.Value;
            lastPosition = rb.position;
            rb.AddForce(direction * BulletData.BulletSpeed, ForceMode.Impulse);
        }

        protected virtual void Explode(Rigidbody otherRB)
        {
            Vector3 currentPosition = rb.position;
            Vector3 direction = currentPosition - lastPosition;
            
            float explosionForce = 3 * BulletData.Damage * Random.value;
            
            otherRB.AddForce(direction.normalized * explosionForce, ForceMode.Impulse);
        }

        protected virtual void ApplyDamage(params Entity[] entities)
        {
            entities ??= Array.Empty<Entity>();
            for (int i = 0; i < entities.Length; i++)
            {
                Entity entity = entities[i];
                if (entity == Author) continue;
                if (entity.TakeDamage(damage) && entity.TryGetComponent(out RayfireRigid rfr))
                {
                    foreach (RayfireRigid frag in rfr.fragments)
                    {
                        if (frag.TryGetComponent(out Rigidbody rfRb))
                        {
                            Explode(rfRb);
                        }
                    }
                }
            }
        }

        protected virtual void HitObject(RaycastHit hit)
        {
            HitObject(hit.collider.gameObject);
        }

        protected virtual void HitObject(GameObject gm)
        {
            if (gm.name == Author.name) return;
            if (gm.TryGetComponent(out RayfireRigid rfr))
            {
                if (rfr.ApplyDamage(50, gm.transform.position, 0.25f))
                {
                    foreach (RayfireRigid frag in rfr.fragments)
                    {
                        if (frag.TryGetComponent(out Rigidbody rfRb))
                        {
                            Explode(rfRb);
                        }
                    }
                }
            }
            
            //+ hit.normal * 0.5f
            BulletData.HitVFX.PlayVFX(gm.transform.position, 2);
            
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