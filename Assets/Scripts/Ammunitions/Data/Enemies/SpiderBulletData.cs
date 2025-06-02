using UnityEngine;

namespace DeadLink.Ammunitions.Data.Enemies
{
    [CreateAssetMenu(menuName = "RogueLike/Ammunition/Enemies/SpiderBulletData")]
    public class SpiderBulletData : BulletData
    {
        [field: SerializeField] public float DamageRadius { get; private set; } = 1f;
    }
}