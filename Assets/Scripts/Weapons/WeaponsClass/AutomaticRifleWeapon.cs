using DeadLink.Entities;
using UnityEngine;

namespace DeadLink.Weapons.WeaponsClass
{
    public class AutomaticRifleWeapon : Weapon
    {
        public override void Fire(Entity entity, Vector3 direction)
        {
            base.Fire(entity, direction);
        }

        public override void Reload()
        {
            
        }
    }
}