using System;
using DeadLink.Entities.Data;
using Enemy;
using KBCore.Refs;
using LTX.ChanneledProperties;
using RayFire;
using UnityEngine;
using UnityEngine.VFX;

namespace DeadLink.Entities
{
    [RequireComponent(typeof(RayfireRigid))]
    public abstract class Enemy : Entity
    {
        [field: SerializeField] public int Cost { get; private set; }
        
        [SerializeField, Self] private RayfireRigid rayfireRigid;

        private void OnValidate() => this.ValidateRefs();
        

        public override void Spawn(EntityData entityData, DifficultyData difficultyData, Vector3 SpawnPosition)
        {
            //Debug.Log("Spawn 1 enemy");
            base.Spawn(entityData, difficultyData, SpawnPosition);
            
            MaxHealth.AddInfluence(difficultyData, difficultyData.EnemyHealthMultiplier, Influence.Multiply);
            Strength.AddInfluence(difficultyData, difficultyData.EnemyStrengthMultiplier, Influence.Multiply);
            Speed.AddInfluence(difficultyData, difficultyData.EnemyStrengthMultiplier, Influence.Multiply);
            
            SetFullHealth();
        }

        public override void Die()
        {
            rayfireRigid.Demolish();
            EnemyManager.Instance.EnemyKilled(this);
        }
    }
}