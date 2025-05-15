using System.Collections;
using DeadLink.Entities.Data;
using DeadLink.PowerUpSystem.InterfacePowerUps;
using Enemy;
using LTX.ChanneledProperties;
using RayFire;
using RogueLike;
using RogueLike.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DeadLink.Entities
{
    public class CustomEnemyBehavior : Entity
    {
        [field: SerializeField] public EnemyUI enemyUI { get; private set; }
        
        [Header("References")]
        [SerializeField] private RayfireRigid rayfireRigid;
        
        private RogueLike.Entities.PlayerEntity currentPlayerEntity;
        private bool canSeePlayer;

        #region spawn damge heal die
        
        public override void Spawn(EntityData entityData, DifficultyData difficultyData, Vector3 SpawnPosition)
        {
            //Debug.Log("Spawn 1 enemy");
            base.Spawn(entityData, difficultyData, SpawnPosition);
            
            MaxHealth.AddInfluence(difficultyData, difficultyData.EnemyHealthMultiplier, Influence.Multiply);
            Strength.AddInfluence(difficultyData, difficultyData.EnemyStrengthMultiplier, Influence.Multiply);
            Speed.AddInfluence(difficultyData, difficultyData.EnemyStrengthMultiplier, Influence.Multiply);
            
            OutlinerManager.Instance.AddOutline(gameObject);
        }
        
        public override bool TakeDamage(float damage)
        {
            bool die = base.TakeDamage(damage);
            enemyUI.UpdateHealthBar(Health, MaxHealth.Value);
            return die;
        }

        public override IEnumerator Die()
        {
            rayfireRigid.Demolish();
            rayfireRigid.Fade();
            OutlinerManager.Instance.RemoveOutline(gameObject);
            
            yield return new WaitForSeconds(0.5f);
            
            AudioManager.Global.PlayOneShot(GameMetrics.Global.FMOD_EnemiesDeath, transform.position);
            //EnemyManager.Instance.EnemyKilled(this);
            Destroy(gameObject);
        }
        
        #endregion
        
        #region Power Ups

        public override void OnUpdate()
        {
        }

        public override void Unlock(IVisitor visitor)
        {
            throw new System.NotImplementedException();
        }

        public override void UsePowerUp(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }
        
        #endregion
        
        protected override void Attack()
        {
            
        }

    }
}