using UnityEngine;
using UnityEngine.VFX;

namespace DeadLink.Entities.Data
{
    public abstract class EntityData : ScriptableObject
    {
        [field: Header("Base Stats")]
        [field: SerializeField]
        public int BaseHealth { get; private set; }
        [field: SerializeField]
        public int BaseHealthBarAmount { get; private set; }
        [field: SerializeField]
        public float BaseSpeed { get; private set; }
        [field: SerializeField]
        public float BaseResistance { get; private set; }
        [field: SerializeField]
        public int BaseStrength { get; private set; }
        
        [field: Header("VFX")]
        [field: SerializeField] public VisualEffect VFXToSpawn { get; private set; }
        [field: SerializeField] public float DelayAfterDestroyVFX { get; private set; }
    }
}