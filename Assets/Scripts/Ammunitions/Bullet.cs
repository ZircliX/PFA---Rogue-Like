using DeadLink.Ammunitions.Data;
using DeadLink.Cameras;
using DeadLink.Entities;
using KBCore.Refs;
using LTX.ChanneledProperties;
using UnityEngine;

namespace DeadLink.Ammunitions
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Bullet : MonoBehaviour
    {
        [field: SerializeField] public BulletData BulletData { get; private set; }
        [SerializeField, Self] private Rigidbody rb;
        
        private float damage;
        private Vector3 lastPosition;

        private void OnValidate() => this.ValidateRefs();
        
        private void Awake()
        {
            CameraController.Instance.CameraShakeProperty.AddPriority(this, PriorityTags.Default);
        }
        
        /*
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Entity entity))
            {
                Debug.Log("Hit an entity");
                HitEntity(entity);
            }
            else
            {
                Debug.Log("Hit something else");
                Hit();
            }
        }
        */

        protected virtual void FixedUpdate()
        {
            Vector3 currentPosition = transform.position;
            Vector3 direction = currentPosition - lastPosition;
            float distance = direction.magnitude;
            
            if (distance > 0)
            {
                Ray ray = new Ray(lastPosition, direction.normalized);

                if (Physics.Raycast(ray, out RaycastHit hit, distance))
                {
                    if (hit.collider.TryGetComponent(out Entity entity))
                    {
                        Debug.Log($"Hit Entity {entity.name}");
                        HitEntity(entity, hit);
                    }
                    else if (hit.collider.gameObject.GetInstanceID() != gameObject.GetInstanceID())
                    {
                        Debug.Log($"Hit object {hit.collider.name}");
                        Hit(hit);
                    }
                }
                else
                {
                    Debug.Log("Didn't hit anything");
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
        
        protected virtual void HitEntity(Entity entity, RaycastHit hit)
        {
            entity.TakeDamage(damage);
            Hit(hit);
        }

        protected virtual void Hit(RaycastHit hit)
        {
            CameraController.Instance.CameraShakeProperty.Write(this, BulletData.CameraShake);
            Instantiate(BulletData.HitVFX, hit.point, Quaternion.identity);
            
            CameraController.Instance.CameraShakeProperty.RemovePriority(this);
            Destroy(gameObject);
        }
    }
}