using DeadLink.Ammunitions.Data;
using UnityEngine;

namespace DeadLink.Ammunitions
{
    public abstract class Bullet : MonoBehaviour
    {
        [field: SerializeField] public BulletData BulletData { get; private set; }
        
        public virtual void Hit()
        {
            
        }
    }
}