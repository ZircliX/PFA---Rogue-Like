using DeadLink.Ammunitions.Data;
using UnityEngine;

namespace DeadLink.Ammunitions
{
    public class LaserBullet : Bullet
    {
        [SerializeField] private BulletData bulletData;
        public override BulletData BulletData => bulletData;
    }
}