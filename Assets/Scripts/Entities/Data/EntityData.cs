using UnityEngine;

namespace DeadLink.Entities.Data
{
    public abstract class EntityData : ScriptableObject
    {
        [field: SerializeField]
        public int BaseHealth { get; private set; }
        [field: SerializeField]
        public float BaseSpeed { get; private set; }
        [field: SerializeField]
        public int BaseStrength { get; private set; }
    }
}