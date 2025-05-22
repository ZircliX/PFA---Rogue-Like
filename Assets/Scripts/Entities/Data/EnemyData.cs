using UnityEngine;

namespace DeadLink.Entities.Data
{
    [CreateAssetMenu(menuName = "RogueLike/Entities/EnemyData")]
    public class EnemyData : EntityData
    {
        [field: SerializeField] public int Cost { get; private set; }

    }
}