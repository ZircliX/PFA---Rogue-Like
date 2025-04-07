using DeadLink.Weapons.Data;
using UnityEngine;

namespace DeadLink.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [Field : SerializeField] 
        public WeaponsData Data { get; private set; }


        public virtual void Fire()
        {
            
        }

        public virtual void Reload()
        {
            
        }
        
    }
}