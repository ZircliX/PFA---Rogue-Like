using UnityEngine;

namespace DeadLink.Ammunitions.Data
{
    [CreateAssetMenu(menuName = "RogueLike/Ammunition/RocketBulletData")]
    public class RocketBulletData : BulletData
    {
        [field : Header("Specific Stats")]
        [field : SerializeField] public float ExplosionRadius { get; private set; }
    }
}