using System;
using System.Collections.Generic;
using DeadLink.Ammunitions.Data;
using DeadLink.Entities;
using UnityEngine;
using ZLinq;

namespace DeadLink.Ammunitions
{
    public class RocketBullet : Bullet
    {
        [SerializeField] private RocketBulletData rocketBulletData;
        public override BulletData BulletData => rocketBulletData;

        protected override void FixedUpdate()
        {
            Vector3 currentPosition = transform.position;
            Vector3 direction = currentPosition - lastPosition;
            float distance = direction.magnitude;
            
            if (distance > 0)
            {
                Ray ray = new Ray(lastPosition, direction.normalized);
                
                if (Physics.Raycast(ray, out RaycastHit hit, distance))
                {
                    if (hit.collider.gameObject.GetInstanceID() != gameObject.GetInstanceID())
                    {
                        List<Entity> entities = new List<Entity>();
                        RaycastHit[] sphereCastAll = Physics.SphereCastAll(hit.point, rocketBulletData.ExplosionRadius, hit.transform.forward) ?? throw new ArgumentNullException("Physics.SphereCastAll(hit.point, RocketBulletData.ExplosionRadius, hit.transform.forward)");

                        for (int i = 0; i < sphereCastAll.Length; i++)
                        {
                            RaycastHit raycastHit = sphereCastAll[i];
                            if (raycastHit.transform.TryGetComponent(out Entity entity))
                            {
                                entities.Add(entity);
                            }
                        }
                        
                        ApplyDamage(entities.AsValueEnumerable().ToArray());
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
    }
}