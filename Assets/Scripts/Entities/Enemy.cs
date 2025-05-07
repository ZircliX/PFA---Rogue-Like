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
        private bool findingNewPos;
        #endregion
        
        [Header("References")]
        [SerializeField, Self] private RayfireRigid rayfireRigid;
        [SerializeField, Self] private NavMeshAgent navMeshAgent;
        
        private Entity player;
        private Vector3 targetPosition;
        private bool canAttack;
        
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
        
        #endregion

        #region spawn damage heal die
        
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
            OutlinerManager.Instance.RemoveOutline(gameObject);
            EnemyManager.Instance.EnemyKilled(this);
            AudioManager.Global.PlayOneShot(GameMetrics.Global.FMOD_EnemiesDeath, transform.position);
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
            HandleDetection();
            HandleIdleMovement();
            
            Attack();
            Move();
        }
        
        #endregion

        private void HandleDetection()
        {
            bool hasVision = HasVisionOnPlayer();

            if (hasVision)
            {
                Debug.Log("Visionnnnnnnnnnn");
                canAttack = inAttackRange;
                
                if (inAggroRange)
                {
                    Vector3 direction = (player.transform.position - transform.position).normalized;
                    Vector3 deltaOffset = direction * attackRadius;

                    targetPosition = player.transform.position - deltaOffset;
                }
            }
        }
        
        protected override void Attack()
        {
            if (!canAttack) return;
            
            player.TakeDamage(1);
        }

        private void Move()
        {
            navMeshAgent.SetDestination(targetPosition);
        }
        
        private void HandleIdleMovement()
        {
            if (inAggroRange || findingNewPos) return;

            bool shouldFindNew = navMeshAgent.velocity.sqrMagnitude < 0.1f;
            if (shouldFindNew)
            {
                findingNewPos = true;
                FindNewPosition();
            }
        }

        private void FindNewPosition()
        {
            Vector3 candidateDirection = Vector3.zero;
            bool found = false;
            
            Vector3 gravity = LevelManager.Instance.PlayerMovement.Gravity.Value.normalized;
            if (gravity == Vector3.zero)
                gravity = Vector3.down;
            
            for (int i = 0; i < 5; i++)
            {
                Vector3 randomPoint = Random.insideUnitSphere * idleMoveRadius;
                Vector3 projected = Vector3.ProjectOnPlane(randomPoint, gravity).normalized;

                if (projected == Vector3.zero)
                    continue;

                float angle = Vector3.Angle(lastDirection, projected);

                if (angle <= maxTurnAngle)
                {
                    candidateDirection = projected;
                    found = true;
                    break;
                }
            }
            
            if (!found)
            {
                for (int i = 0; i < 5; i++)
                {
                    Vector3 randomPoint = Random.insideUnitSphere * idleMoveRadius;
                    candidateDirection = Vector3.ProjectOnPlane(randomPoint, gravity).normalized;
                    if (candidateDirection != Vector3.zero)
                    {
                        found = true;
                        break;
                    }
                }
            }
            
            if (found)
            {
                Vector3 target = transform.position + candidateDirection * idleMoveRadius;
                if (NavMesh.SamplePosition(target, out NavMeshHit hit, 1, NavMesh.AllAreas))
                {
                    targetPosition = hit.position;
                    lastDirection = (hit.position - transform.position).normalized;
                    return;
                }
            }
            
            findingNewPos = false;
        }

        private bool HasVisionOnPlayer()
        {
            if (player == null) return false;
            
            Vector3 deltaPosition = (player.transform.position - transform.position);
            Vector3 direction = deltaPosition.normalized;
            float distance = deltaPosition.magnitude;
            
            bool objectBlocking = Physics.Raycast(transform.position, direction, out RaycastHit hit,
                distance, GameMetrics.Global.EnemyStopDetect);

            if (objectBlocking) return false;

            Debug.DrawLine(transform.position, player.transform.position, Color.black);
            
            float angle = Vector3.Dot(transform.forward, direction.normalized);
            bool correctAngle = angle < -0.7f;
            
            return correctAngle;
        }
        
        #region Trigger Events
        
        private void TriggerEnter(Collider other, SphereDetector sphereDetector)
        {
            if (other.TryGetComponent(out RogueLike.Entities.Player player))
            {
                this.player = player;
                
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
            
        }
        
        private void TriggerExit(Collider other, SphereDetector sphereDetector)
        {
            if (other.TryGetComponent(out RogueLike.Entities.Player player))
            {
                if (sphereDetector == detectDetector)
                {
                    inDetectRange = false;
                    this.player = null;
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