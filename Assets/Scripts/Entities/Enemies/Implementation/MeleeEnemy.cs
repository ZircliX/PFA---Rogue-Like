using DeadLink.PowerUpSystem.InterfacePowerUps;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace DeadLink.Entities.Enemies
{
    public class MeleeEnemy : Enemy
    {
        #region NavMesh Params
        [Header("NavMesh Parameters")]
        [SerializeField] protected float moveSpeed = 3.5f;
        
        [Header("Idle Patrol Behavior")]
        [SerializeField] private float patrolActivityRadius = 10f; // How far from its origin it will patrol
        [SerializeField] private float minPatrolWaitTime = 2f;
        [SerializeField] private float maxPatrolWaitTime = 5f;
        
        private Vector3 _patrolOrigin;
        private Vector3 _currentPatrolDestination;
        private float _patrolWaitTimer;
        private bool _isMovingToPatrolDestination;

        #endregion
        
        [SerializeField, Self] private NavMeshAgent navMeshAgent;
        
        private Vector3 targetPosition;

        protected override void Start()
        {
            base.Start();
            _patrolOrigin = transform.position;
            _isMovingToPatrolDestination = false;
            _patrolWaitTimer = Random.Range(minPatrolWaitTime, maxPatrolWaitTime) / 2f;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            HandleMovement();
        }

        protected override void HandleOrientation()
        {
            return;
            if (HasVisionOnPlayer())
            {
                Vector3 directionToPlayer = player.transform.position - transform.position;
                directionToPlayer.y = 0; // Makes the enemy only rotate on its Y-axis (stay upright)

                if (directionToPlayer.sqrMagnitude > 0.001f) // Check if direction is not zero
                {
                    Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, chaseRotationSpeed * Time.deltaTime);
                }
            }
        }

        private void HandleMovement()
        {
            if (player != null && player.gameObject.activeInHierarchy)
            {
                // Log current states to debug
                float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
                // Debug.Log($"PlayerDist: {distanceToPlayer:F1}, Detect:{inDetectRange}, Aggro:{inAggroRange}, Attack:{inAttackRange}, CanAttack:{canAttack}");

                if (inAggroRange) // Player is close enough to be aggressive towards
                {
                    StopPatrolAndNavMesh(); // Interrupt patrol if it was happening
                    OrientTowardsTarget(player.transform.position, chaseRotationSpeed);
                    FollowPlayer(); // This will handle movement and stopping at attack range
            
                    // Attack logic would typically be checked here too
                    // if (inAttackRange && canAttack) {
                    //     PerformAttack();
                    // }
                }
                else if (inDetectRange) // Player is detected, but not close enough for aggro
                {
                    StopPatrolAndNavMesh();
                    OrientTowardsTarget(player.transform.position, chaseRotationSpeed); // "just watches him"
                    if (navMeshAgent != null && navMeshAgent.enabled && navMeshAgent.hasPath)
                    {
                        navMeshAgent.ResetPath(); // Stop any NavMeshAgent movement if it was patrolling
                    }
                }
                else // Player might be known but is outside detect range now
                {
                    PerformIdlePatrol();
                }
            }
            else // No player target at all
            {
                PerformIdlePatrol();
            }
        }
        
        void StopPatrolAndNavMesh()
        {
            _isMovingToPatrolDestination = false; // Stop logical patrol state
            _patrolWaitTimer = 0f;
            return;
            if (navMeshAgent != null && navMeshAgent.enabled && navMeshAgent.hasPath)
            {
                navMeshAgent.ResetPath(); // Stop NavMeshAgent movement
                navMeshAgent.velocity = Vector3.zero; // Ensure it stops immediately
            }
        }
        
        void PerformIdlePatrol()
        {
            if (navMeshAgent != null && navMeshAgent.enabled && navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.speed = moveSpeed; // Or use a dedicated patrolMoveSpeed
                if (!navMeshAgent.hasPath && navMeshAgent.remainingDistance < 0.5f) // If not moving or reached destination
                {
                    _isMovingToPatrolDestination = false; // Arrived
                }
            }


            if (_isMovingToPatrolDestination)
            {
                // If not using NavMeshAgent, handle movement and orientation:
                if (navMeshAgent == null || !navMeshAgent.enabled)
                {
                    Vector3 directionToDestination = _currentPatrolDestination - transform.position;
                    directionToDestination.y = 0;

                    if (directionToDestination.sqrMagnitude > 0.2f * 0.2f) // If not very close
                    {
                        OrientTowardsTarget(_currentPatrolDestination, chaseRotationSpeed);
                        transform.position += directionToDestination.normalized * moveSpeed * Time.deltaTime; // Or patrolMoveSpeed
                    }
                    else // Reached destination (for non-NavMeshAgent)
                    {
                        _isMovingToPatrolDestination = false;
                        _patrolWaitTimer = Random.Range(minPatrolWaitTime, maxPatrolWaitTime);
                    }
                }
                // If NavMeshAgent is handling movement, it will stop when it reaches.
                // We check navMeshAgent.remainingDistance above.
            }
            else // Waiting at a spot or NavMeshAgent has arrived
            {
                _patrolWaitTimer -= Time.deltaTime;
                if (_patrolWaitTimer <= 0)
                {
                    SetNewPatrolDestination();
                }
                // Optionally, you could add a slow random "look around" rotation here while waiting.
            }
        }

        void SetNewPatrolDestination()
        {
            float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            Vector3 randomDirection = new Vector3(Mathf.Sin(randomAngle), 0, Mathf.Cos(randomAngle));
            float randomDistance = Random.Range(patrolActivityRadius * 0.1f, patrolActivityRadius); // Don't always go to the edge
            _currentPatrolDestination = _patrolOrigin + randomDirection * randomDistance;

            _isMovingToPatrolDestination = true;

            if (navMeshAgent != null && navMeshAgent.enabled && navMeshAgent.isOnNavMesh)
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(_currentPatrolDestination, out hit, patrolActivityRadius * 0.5f, NavMesh.AllAreas)) // Sample within a reasonable radius
                {
                    navMeshAgent.SetDestination(hit.position);
                }
                else
                {
                    // Failed to find a point on the NavMesh, try picking another point or wait again
                    _isMovingToPatrolDestination = false;
                    _patrolWaitTimer = Random.Range(minPatrolWaitTime, maxPatrolWaitTime) / 2f; // Shorter wait before retry
                    //Debug.LogWarning("Failed to sample NavMesh position for patrol. Retrying soon.", this);
                }
            }
            // If not using NavMeshAgent, _currentPatrolDestination is used directly by transform.position update.
        }


        void OrientTowardsTarget(Vector3 targetPosition, float rotationSpeed)
        {
            Vector3 direction = targetPosition - transform.position;
            direction.y = 0; // Keep enemy upright

            if (direction.sqrMagnitude > 0.001f) // If not looking practically at the same spot
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        
        void FollowPlayer()
        {
            if (player == null) return;

            // If already in attack range, stop moving closer to allow attacks.
            if (inAttackRange)
            {
                if (navMeshAgent != null && navMeshAgent.enabled)
                {
                    // Stop the agent, but it should already be oriented by HandleBehavior
                    if (!navMeshAgent.isStopped) navMeshAgent.isStopped = true;
                }
                // If not using NavMeshAgent, no explicit stop is needed here as movement below won't execute.
                return; // Stop further movement logic
            }

            // If using NavMeshAgent for following
            if (navMeshAgent != null && navMeshAgent.enabled && navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.isStopped = false;
                navMeshAgent.speed = moveSpeed; // Ensure using chase speed
                Debug.Log(navMeshAgent.isStopped);
                Debug.Log(navMeshAgent.remainingDistance);
                navMeshAgent.SetDestination(player.transform.position);
            }
            else // Manual transform-based movement
            {
                Vector3 directionToPlayer = player.transform.position - transform.position;
                directionToPlayer.y = 0; // Keep movement planar

                // Check distance again to ensure we don't overshoot into attackRadius if NavMeshAgent isn't used
                float distanceToPlayer = directionToPlayer.magnitude;
                if (distanceToPlayer > attackRadius) // Only move if still outside attack radius
                {
                    transform.position += directionToPlayer.normalized * moveSpeed * Time.deltaTime;
                }
            }
        }

        public override void Unlock(IVisitor visitor)
        {
        }

        public override void UsePowerUp(InputAction.CallbackContext context)
        {
        }
        
    }
}