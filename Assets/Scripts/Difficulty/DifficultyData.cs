using System;
using EditorAttributes;
using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(menuName = "RogueLike/Difficulty")]
    public class DifficultyData : ScriptableObject
    {
        [field: SerializeField, ReadOnly] public string GUID { get; private set; }

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(GUID))
            {
                GUID = Guid.NewGuid().ToString();
            }
        }
        
        #region player
        
        [field : SerializeField]
        public float PlayerHealthMultiplier { get; private set; }
        
        [field : SerializeField]
        public float PlayerStrengthMultiplier { get; private set; }
        
        [field : SerializeField]
        public float PlayerResistanceMultiplier { get; private set; }
        
        [field : SerializeField]
        public int PlayerHealthBarCount { get; private set; }
        #endregion
        
        #region Enemy
        
        [field : SerializeField]
        public int WaveBalanceMultiplier { get; private set; }
        
        [field : SerializeField]
        public float EnemyStrengthMultiplier { get; private set; }
        
        [field : SerializeField]
        public float EnemyHealthMultiplier { get; private set; }
        
        #endregion 
    }
}