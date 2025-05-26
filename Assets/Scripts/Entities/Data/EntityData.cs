using System;
using EditorAttributes;
using RogueLike.VFX;
using UnityEngine;
using UnityEngine.VFX;

namespace DeadLink.Entities.Data
{
    public abstract class EntityData : ScriptableObject
    {
        [field: Header("Enemy")]
        [field: SerializeField] public Entity EntityPrefab { get; private set; }
        [field: SerializeField, ReadOnly] public string GUID { get; private set; } = Guid.NewGuid().ToString();
        
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
        public float BaseStrength { get; private set; }
        
        [field: Header("VFX")]
        [field: SerializeField] public VfxComponent VFXToSpawn { get; private set; }
        [field: SerializeField] public float DelayAfterDestroyVFX { get; private set; }


        private void OnValidate()
        {
            if (string.IsNullOrEmpty(GUID))
            {
                GUID = Guid.NewGuid().ToString();
            }
        }
    }
}