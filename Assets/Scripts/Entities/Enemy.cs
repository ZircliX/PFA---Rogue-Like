using System;
using System.Collections.Generic;
using DeadLink.Entities.Data;
using Enemy;
using KBCore.Refs;
using LTX.ChanneledProperties;
using RayFire;
using RogueLike.Controllers;
using UnityEngine;

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
            
            OutlinerManager.Instance.AddOutline(gameObject);
        }

        float currentTime = 0;
        float maxTime = 0.5f;
        
        protected override void Update()
        {
            base.Update();
            
            if (currentTime >= maxTime)
            {
                currentTime = 0;
                
                Ray ray = new Ray(transform.position, transform.forward);
                RaycastHit[] raycast = Physics.SphereCastAll(ray, 3, 3);
                
                for (int i = 0; i < raycast.Length; i++)
                {
                    if (raycast[i].transform.TryGetComponent(out RogueLike.Entities.Player player))
                    {
                        player.TakeDamage(1);
                    }
                }
            }
            else
            {
                currentTime += Time.deltaTime;
            }
        }

        public override void Die()
        {
            rayfireRigid.Demolish();
            OutlinerManager.Instance.RemoveOutline(gameObject);
            EnemyManager.Instance.EnemyKilled(this);
            Destroy(gameObject);
        }
    }
}