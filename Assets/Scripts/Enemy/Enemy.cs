using LTX.ChanneledProperties;
using RogueLike.Controllers;
using UnityEngine;
using UnityEngine.VFX;

namespace Enemy
{
    public abstract class Enemy : Entity
    {
        [field: SerializeField] public VisualEffect VFXToSpawn { get; private set; }
        [field: SerializeField] public float DelayAfterDestroyVFX { get; private set; }
        
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