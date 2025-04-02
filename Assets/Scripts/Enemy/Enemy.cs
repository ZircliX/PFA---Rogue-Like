using LTX.ChanneledProperties;
using UnityEngine;

namespace Enemy
{
    public abstract class Enemy : Entity
    {
        
        public override void Spawn(EntityData entityData, DifficultyData difficultyData)
        {
            base.Spawn(entityData, difficultyData);
            
            MaxHealth.AddInfluence(difficultyData, difficultyData.EnemyHealthMultiplier, Influence.Multiply);
            Strength.AddInfluence(difficultyData, difficultyData.EnemyStrengthMultiplier, Influence.Multiply);
            Speed.AddInfluence(difficultyData, difficultyData.EnemyStrengthMultiplier, Influence.Multiply);
            
            SetFullHealth();
        }
    }
}