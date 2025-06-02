using UnityEngine;

namespace DeadLink.Ammunitions.Data.Enemies
{
    [CreateAssetMenu(menuName = "RogueLike/Ammunition/Enemies/SlashBulletData")]
    public class SlashBulletData : BulletData
    {
        [field: SerializeField] public float DamageRadius { get; private set; } = 1f;
    }
}