using System.Collections;
using DeadLink.Entities.Data;
using Enemy;
using KBCore.Refs;
using LTX.ChanneledProperties;
using RayFire;
using RogueLike;
using RogueLike.Controllers;
using RogueLike.Managers;
using UnityEngine;
using UnityEngine.AI;

namespace DeadLink.Entities
{
    [RequireComponent(typeof(SphereCollider))]
    public abstract class Enemy : Entity
    {
        [field: SerializeField] public int Cost { get; private set; }
        [field: SerializeField] public EnemyUI enemyUI { get; private set; }
        
        [Header("Detection Parameters")]
        [SerializeField] private float sphereRadius = 5f;
        
        [Header("NavMesh Parameters")]
        [SerializeField] private float newPosRadius = 5f;
        [SerializeField] private float maxTurnAngle = 90f;
        [SerializeField] private float idleTime = 1f;

        private Vector3 lastDirection = Vector3.forward;
        private bool isRandomMoving = false;
        
        [Header("References")]
        [SerializeField, Self] private RayfireRigid rayfireRigid;
        [SerializeField, Self] private NavMeshAgent navMeshAgent;
        [SerializeField, Self] private SphereCollider sc;

        private RogueLike.Entities.Player currentPlayer;
        private bool canSeePlayer;
        
        private void OnValidate()
        {
            this.ValidateRefs();
            sc.radius = sphereRadius;
        }

        private void Start()
        {
            StartCoroutine(MoveRoutine());
        }

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

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            enemyUI.UpdateHealthBar(Health, MaxHealth.Value);
        }

        public override void Die()
        {
            rayfireRigid.Demolish();
            rayfireRigid.Fade();
            OutlinerManager.Instance.RemoveOutline(gameObject);
            EnemyManager.Instance.EnemyKilled(this);
            AudioManager.Global.PlayOneShot(GameMetrics.Global.FMOD_EnemiesDeath, transform.position);
            Destroy(gameObject);
        }

        protected void FixedUpdate()
        {
            ConeCast();
            CalculatePlayerPosition();
            MoveTowardPlayer();
        }
        
        private IEnumerator MoveRoutine()
        {
            while (true)
            {
                if (!isRandomMoving && !canSeePlayer)
                {
                    yield return new WaitForSeconds(idleTime);
                    TryMove();
                }
                yield return null;
            }
        }
        
        private void TryMove()
        {
            Vector3 candidateDirection = Vector3.zero;
            bool found = false;

            for (int i = 0; i < 10; i++)
            {
                Vector3 randomPoint = Random.insideUnitSphere * newPosRadius;
                Vector3 projected = Vector3.ProjectOnPlane(randomPoint, LevelManager.Instance.PlayerMovement.Gravity.Value.normalized);

                float angle = Vector3.Angle(lastDirection, projected);

                if (angle <= maxTurnAngle)
                {
                    candidateDirection = projected;
                    found = true;
                    break;
                }
            }

            if (found)
            {
                Vector3 target = transform.position + candidateDirection * newPosRadius;
                if (NavMesh.SamplePosition(target, out NavMeshHit hit, 1, NavMesh.AllAreas))
                {
                    navMeshAgent.SetDestination(hit.position);
                    lastDirection = (hit.position - transform.position).normalized;
                    isRandomMoving = true;
                    StartCoroutine(WaitUntilStopped());
                }
            }
        }

        private IEnumerator WaitUntilStopped()
        {
            while (navMeshAgent.pathPending || navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
            {
                yield return null;
            }

            isRandomMoving = false;
        }

        private void MoveTowardPlayer()
        {
            if (currentPlayer != null && canSeePlayer)
            {
                navMeshAgent.SetDestination(currentPlayer.transform.position); 
                Attack();
            }
        }

        private void CalculatePlayerPosition()
        {
            if (currentPlayer == null)
            {
                canSeePlayer = false;
                return;
            }
            
            bool didHit = false;
            RaycastHit hit = default;

            Vector3 deltaPosition = (currentPlayer.transform.position - transform.position);
            Vector3 direction = deltaPosition.normalized;
            float distance = deltaPosition.magnitude;
            
            //Debug.DrawRay(transform.position, direction * distance, Color.cyan, 2);
            didHit = Physics.Raycast(transform.position, direction, out hit,
                distance, GameMetrics.Global.EnemyStopDetect);

            canSeePlayer = !didHit;
        }

        private void ConeCast()
        {
            RaycastHit[] hits = PhysicsExtensions.ConeCastAll(transform.position, 3, transform.forward, 15, 35f);
            
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.TryGetComponent(out RogueLike.Entities.Player player))
                {
                    currentPlayer = player;
                }
            }
        }
    }
}