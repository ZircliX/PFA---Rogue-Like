using System.Collections;
using DeadLink.Entities.Data;
using DeadLink.Entities.Enemies.Detection;
using Enemy;
using KBCore.Refs;
using LTX.ChanneledProperties;
using RayFire;
using RogueLike;
using RogueLike.Controllers;
using RogueLike.Managers;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace DeadLink.Entities
{
    public abstract class Enemy : Entity
    {
        [field: SerializeField] public int Cost { get; private set; }
        [field: SerializeField] public EnemyUI enemyUI { get; private set; }
        
        #region Detection Params
        [Header("Detection Parameters")]
        [SerializeField] private float detectRadius = 12.5f;
        [SerializeField] private SphereDetector detectDetector;
        [SerializeField] private float aggroRadius = 10;
        [SerializeField] private SphereDetector aggroDetector;
        [SerializeField] private float attackRadius = 8;
        [SerializeField] private SphereDetector attackDetector;
        protected bool inDetectRange;
        protected bool inAggroRange;
        protected bool inAttackRange;
        #endregion
        
        #region NavMesh Params
        [Header("NavMesh Parameters")]
        [SerializeField] private float idleMoveRadius = 5f;
        [SerializeField] private float maxTurnAngle = 90f;
        [SerializeField] private float idleTime = 1f;

        private Vector3 lastDirection = Vector3.forward;
        private bool isRandomMoving = false;
        #endregion
        
        [Header("References")]
        [SerializeField, Self] private RayfireRigid rayfireRigid;
        [SerializeField, Self] private NavMeshAgent navMeshAgent;
        
        private Transform playerTransform;
        
        #region Event Functions
        
        private void OnValidate()
        {
            this.ValidateRefs();
            detectDetector.SphereCollider.radius = detectRadius;
            aggroDetector.SphereCollider.radius = aggroRadius;
            attackDetector.SphereCollider.radius = attackRadius;
        }

        private void OnEnable()
        {
            detectDetector.OnTriggerEnterEvent += TriggerEnter;
            detectDetector.OnTriggerStayEvent += TriggerStay;
            detectDetector.OnTriggerExitEvent += TriggerExit;
            
            aggroDetector.OnTriggerEnterEvent += TriggerEnter;
            aggroDetector.OnTriggerStayEvent += TriggerStay;
            aggroDetector.OnTriggerExitEvent += TriggerExit;
            
            attackDetector.OnTriggerEnterEvent += TriggerEnter;
            attackDetector.OnTriggerStayEvent += TriggerStay;
            attackDetector.OnTriggerExitEvent += TriggerExit;
        }
        
        private void OnDisable()
        {
            detectDetector.OnTriggerEnterEvent -= TriggerEnter;
            detectDetector.OnTriggerStayEvent -= TriggerStay;
            detectDetector.OnTriggerExitEvent -= TriggerExit;
            
            aggroDetector.OnTriggerEnterEvent -= TriggerEnter;
            aggroDetector.OnTriggerStayEvent -= TriggerStay;
            aggroDetector.OnTriggerExitEvent -= TriggerExit;
            
            attackDetector.OnTriggerEnterEvent -= TriggerEnter;
            attackDetector.OnTriggerStayEvent -= TriggerStay;
            attackDetector.OnTriggerExitEvent -= TriggerExit;
        }

        private void Start()
        {
            StartCoroutine(MoveRoutine());
        }
        
        #endregion

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

        #endregion
        
        #region Updates
        
        protected override void Update()
        {
            base.Update();
            
            /* Debug damage to player
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
            */
        }
        
        public override void OnFixedUpdate()
        {
            HandleMovement();
        }
        
        #endregion
        
        #region Idle Moving Logic
        
        private IEnumerator MoveRoutine()
        {
            while (true)
            {
                if (!isRandomMoving && !inAggroRange)
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
                Vector3 randomPoint = Random.insideUnitSphere * idleMoveRadius;
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
                Vector3 target = transform.position + candidateDirection * idleMoveRadius;
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
        
        #endregion

        private void HandleMovement()
        {
            if (playerTransform != null && HasVisionOnPlayer() &&!isRandomMoving)
            {
                navMeshAgent.SetDestination(playerTransform.position); 
            }
        }

        private bool HasVisionOnPlayer()
        {
            if (playerTransform == null) return false;
            
            Vector3 deltaPosition = (playerTransform.position - transform.position);
            Vector3 direction = deltaPosition.normalized;
            float distance = deltaPosition.magnitude;
            
            bool didHit = Physics.Raycast(transform.position, direction, out RaycastHit hit,
                distance, GameMetrics.Global.EnemyStopDetect);

            return !didHit;
        }
        
        #region Trigger Events
        
        private void TriggerEnter(Collider other, SphereDetector sphereDetector)
        {
            if (other.TryGetComponent(out RogueLike.Entities.Player player))
            {
                playerTransform = player.transform;
                
                if (sphereDetector == detectDetector)
                {
                    inDetectRange = true;
                }
                else if (sphereDetector == aggroDetector)
                {
                    inAggroRange = true;
                }
                else if (sphereDetector == attackDetector)
                {
                    inAttackRange = true;
                }
            }
        }
        
        private void TriggerStay(Collider other, SphereDetector sphereDetector)
        {
            if (other.TryGetComponent(out RogueLike.Entities.Player player))
            {
                if (sphereDetector == detectDetector)
                {
                    inDetectRange = true;
                }
                else if (sphereDetector == aggroDetector)
                {
                    inAggroRange = true;
                }
                else if (sphereDetector == attackDetector)
                {
                    inAttackRange = true;
                }
            }
        }
        
        private void TriggerExit(Collider other, SphereDetector sphereDetector)
        {
            if (other.TryGetComponent(out RogueLike.Entities.Player player))
            {
                if (sphereDetector == detectDetector)
                {
                    inDetectRange = false;
                    playerTransform = null;
                }
                else if (sphereDetector == aggroDetector)
                {
                    inAggroRange = false;
                }
                else if (sphereDetector == attackDetector)
                {
                    inAttackRange = false;
                }
            }
        }
        
        #endregion
    }
}