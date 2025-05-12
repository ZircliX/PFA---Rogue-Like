using DeadLink.Entities.Data;
using DeadLink.Entities.Enemies.Detection;
using Enemy;
using KBCore.Refs;
using LTX.ChanneledProperties;
using RayFire;
using RogueLike;
using RogueLike.Controllers;
using UnityEngine;

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
        
        [Header("References")]
        [SerializeField, Self] private RayfireRigid rayfireRigid;
        
        
        protected Entity player;
        protected bool canAttack;
        
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
        
        public override void OnFixedUpdate()
        {
            HandleDetection();
            HandleOrientation();
            Attack();
        }
        
        #endregion

        private void HandleDetection()
        {
            bool hasVision = HasVisionOnPlayer();
            canAttack = inAttackRange && hasVision;
            
            if (hasVision)
            {
                if (inAggroRange)
                {
                    Vector3 direction = (player.transform.position - transform.position).normalized;
                    Vector3 deltaOffset = direction * attackRadius;

                    //targetPosition = player.transform.position - deltaOffset;
                }
            }
        }
        
        private void HandleOrientation()
        {
            if (!HasVisionOnPlayer()) return;
            
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 5f);
        }
        
        protected override void Attack()
        {
            if (!canAttack) return;
            
            player.TakeDamage(1);
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
            if (other.TryGetComponent(out RogueLike.Entities.Player playerDetected))
            {
                if (sphereDetector == detectDetector)
                {
                    Debug.Log("Enter detect range");
                    this.player = playerDetected;
                    inDetectRange = true;
                }
                else if (sphereDetector == aggroDetector)
                {
                    Debug.Log("Enter aggro range");
                    inAggroRange = true;
                }
                else if (sphereDetector == attackDetector)
                {
                    Debug.Log("Enter attack range");
                    inAttackRange = true;
                }
            }
        }
        
        private void TriggerStay(Collider other, SphereDetector sphereDetector)
        {
            
        }
        
        private void TriggerExit(Collider other, SphereDetector sphereDetector)
        {
            if (other.TryGetComponent(out RogueLike.Entities.Player playerDetected))
            {
                if (sphereDetector == detectDetector)
                {
                    Debug.Log("Exit detect range");
                    inDetectRange = false;
                    this.player = null;
                }
                else if (sphereDetector == aggroDetector)
                {
                    Debug.Log("Exit aggro range");
                    inAggroRange = false;
                }
                else if (sphereDetector == attackDetector)
                {
                    Debug.Log("Exit attack range");
                    inAttackRange = false;
                }
            }
        }
        
        #endregion
    }
}