using DeadLink.Weapons.Data;
using UnityEngine;

namespace DeadLink.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [field : SerializeField] public WeaponData WeaponData { get; private set; }
        
        public virtual void Fire()
        {
            
        }

        public virtual void Reload()
        {
            
        }
    }
}