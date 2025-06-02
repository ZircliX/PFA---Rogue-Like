using System;
using DeadLink.Ammunitions.Data;
using UnityEngine;

namespace DeadLink.Ammunitions.Enemies
{
    public class TurretBullet : Bullet
    {
        public override BulletData BulletData => bulletData;
        [SerializeField] private BulletData bulletData;
    }
}