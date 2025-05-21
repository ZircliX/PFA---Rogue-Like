using System;
using System.Collections;
using DeadLink.Entities.Data;
using DeadLink.Entities.Enemies.Detection;
using DG.Tweening;
using EditorAttributes;
using Enemy;
using KBCore.Refs;
using LTX.ChanneledProperties;
using RayFire;
using RogueLike;
using RogueLike.Controllers;
using UnityEngine;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace DeadLink.Entities
{
    public abstract class Enemy : Entity
    {
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

        public GameObject outline;
        
        protected Entity player;
        protected bool canAttack;
        
        [field: SerializeField, ReadOnly, HideInEditMode] public string GUID { get; private set; }
        
        #region Event Functions
        
        private void OnValidate()
        {
            this.ValidateRefs();
            detectDetector.SphereCollider.radius = detectRadius;
            aggroDetector.SphereCollider.radius = aggroRadius;
            attackDetector.SphereCollider.radius = attackRadius;
            
#if UNITY_EDITOR
            PrefabStage currentPrefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (currentPrefabStage != null && currentPrefabStage.IsPartOfPrefabContents(gameObject))
            {
                //Debug.Log($" Prefab Instance: {currentPrefabStage.prefabContentsRoot == gameObject}", gameObject);
                SetGUID(string.Empty);
            }
            else if (PrefabUtility.IsPartOfPrefabAsset(gameObject) || EditorUtility.IsPersistent(gameObject))
            {
                //Debug.Log($" Prefab Asset: {PrefabUtility.IsPartOfPrefabAsset(gameObject)}", gameObject);
                SetGUID(string.Empty);
            }
            else if (string.IsNullOrEmpty(GUID))
            {
                //Debug.Log($"Assigning GUID: {GUID} to {gameObject.name}", gameObject);
                SetGUID(Guid.NewGuid().ToString());
            }
#endif
        }
        
        private void OnEnable()
        {
            EnemyManager.Instance.RegisterEnemy(this);
            
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
            EnemyManager.Instance.UnregisterEnemy(this);
            
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
            if (string.IsNullOrEmpty(GUID))
            {
                SetGUID(Guid.NewGuid().ToString());
            }
        }

        public void SetGUID(string guid) => GUID = guid;

        #endregion

        #region spawn damage heal die
        
        public override void Spawn(EntityData entityData, DifficultyData difficultyData, Vector3 SpawnPosition)
        {
            //Debug.Log("Spawn 1 enemy");
            base.Spawn(entityData, difficultyData, SpawnPosition);
            
            MaxHealthBarCount.AddInfluence(difficultyData, difficultyData.PlayerHealthBarCount, Influence.Multiply);
            MaxHealth.AddInfluence(difficultyData, difficultyData.EnemyHealthMultiplier, Influence.Multiply);
            Strength.AddInfluence(difficultyData, difficultyData.EnemyStrengthMultiplier, Influence.Multiply);
            Resistance.AddInfluence(difficultyData, difficultyData.PlayerResistanceMultiplier, Influence.Multiply);
            Speed.AddInfluence(difficultyData, 1, Influence.Multiply);
            
            OutlinerManager.Instance.AddOutline(gameObject);
            
            SetFullHealth();
        }

        protected override void SetHealth(float health)
        {
            base.SetHealth(health);
            enemyUI.UpdateHealthBar(Health, MaxHealth.Value);
        }

        public override bool TakeDamage(float damage)
        {
            bool die = base.TakeDamage(damage);
            return die;
        }

        public override IEnumerator Die()
        {
            OutlinerManager.Instance.RemoveOutline(gameObject);
            
            yield return new WaitForSeconds(0.25f);
            DOTween.Kill(gameObject);
            
            AudioManager.Global.PlayOneShot(GameMetrics.Global.FMOD_EnemiesDeath, transform.position);
            EntityData.VFXToSpawn.PlayVFX(transform.position, EntityData.DelayAfterDestroyVFX);

            EnemyManager.Instance.EnemyKilled(this);
            rayfireRigid.Demolish();
        }

        #endregion
        
        #region Updates
        
        public override void OnUpdate()
        {
            HandleDetection();
            HandleOrientation();
        }
        
        #endregion
        
        protected override void Shoot()
        {
            if (CurrentWeapon != null && CurrentWeapon.CurrentReloadTime >= CurrentWeapon.WeaponData.ReloadTime)
            {
                GameObject objectToHit = null;
                Vector3 direction = player.transform.position - transform.position + Vector3.Scale(transform.forward, Random.onUnitSphere) * 10;
                Debug.DrawRay(BulletSpawnPoint.position, direction * 50, Color.red, 2);

                if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 500, GameMetrics.Global.BulletRayCast))
                {
                    objectToHit = hit.collider.gameObject;
                }
                
                CurrentWeapon.Fire(this, direction, objectToHit);
            }
            else
            {
                //Debug.LogError($"No equipped weapon for {gameObject.name}");
            }
        }

        private void HandleDetection()
        {
            bool hasVision = HasVisionOnPlayer();
            //Debug.Log(hasVision, this);
            canAttack = inAttackRange && hasVision;
            isShooting = canAttack;
        }
        
        private void HandleOrientation()
        {
            if (!HasVisionOnPlayer()) return;
            
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 5f);
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
            
            float angle = Vector3.Dot(-transform.forward, direction.normalized);
            bool correctAngle = angle < -0.7f;
            
            return correctAngle;
        }
        
        #region Trigger Events
        
        private void TriggerEnter(Collider other, SphereDetector sphereDetector)
        {
            if (other.TryGetComponent(out RogueLike.Entities.PlayerEntity playerDetected))
            {
                if (playerDetected.IsInvisible) return;
                
                if (sphereDetector == detectDetector)
                {
                    //Debug.Log("Enter detect range");
                    this.player = playerDetected;
                    inDetectRange = true;
                }
                else if (sphereDetector == aggroDetector)
                {
                    //Debug.Log("Enter aggro range");
                    inAggroRange = true;
                }
                else if (sphereDetector == attackDetector)
                {
                    //Debug.Log("Enter attack range");
                    inAttackRange = true;
                }
            }
        }
        
        private void TriggerStay(Collider other, SphereDetector sphereDetector)
        {
            
        }
        
        private void TriggerExit(Collider other, SphereDetector sphereDetector)
        {
            if (other.TryGetComponent(out RogueLike.Entities.PlayerEntity playerDetected))
            {
                if (sphereDetector == detectDetector)
                {
                    //Debug.Log("Exit detect range");
                    inDetectRange = false;
                    this.player = null;
                }
                else if (sphereDetector == aggroDetector)
                {
                    //Debug.Log("Exit aggro range");
                    inAggroRange = false;
                }
                else if (sphereDetector == attackDetector)
                {
                    //Debug.Log("Exit attack range");
                    inAttackRange = false;
                }
            }
        }
        
        #endregion
    }
}