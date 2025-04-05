using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(menuName = "RogueLike/Difficulty")]
    public class DifficultyData : ScriptableObject
    {
        #region player
        
        [field : SerializeField]
        public float PlayerHealthMultiplier { get; private set; }
        
        [field : SerializeField]
        public float PlayerStrengthMultiplier { get; private set; }
            
        #endregion
        
        #region Enemy
        
        [field : SerializeField]
        public float EnemyHealthMultiplier { get; private set; }
        
        [field : SerializeField]
        public float EnemyStrengthMultiplier { get; private set; }
        
        #endregion 
    }
}