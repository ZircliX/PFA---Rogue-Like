using LTX.ChanneledProperties;
using RogueLike.Controllers;
using UnityEngine;

namespace Enemy
{
    public abstract class Enemy : Entity
    {
        
        public ParticleSystem vfxToSpawn; 
        public float delayAfterDestroyVfx;
        
        public override void Spawn(EntityData entityData, DifficultyData difficultyData, Vector3 SpawnPosition)
        {
            base.Spawn(entityData, difficultyData, SpawnPosition);
            
            MaxHealth.AddInfluence(difficultyData, difficultyData.EnemyHealthMultiplier, Influence.Multiply);
            Strength.AddInfluence(difficultyData, difficultyData.EnemyStrengthMultiplier, Influence.Multiply);
            Speed.AddInfluence(difficultyData, difficultyData.EnemyStrengthMultiplier, Influence.Multiply);
            
            SetFullHealth();
        }

        public override void Die()
        {
            EnemiesManager.Instance.EnemyKilled(this);
        }
    }
}