using DeadLink.PowerUpSystem.InterfacePowerUps;
using KBCore.Refs;
using RogueLike;
using RogueLike.Managers;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace DeadLink.Entities.Enemies
{
    public class MeleeEnemy : Enemy
    {
        #region NavMesh Params
        [Header("NavMesh Parameters")]
        [SerializeField] private float idleMoveRadius = 5f;
        [SerializeField] private float maxTurnAngle = 90f;
        [SerializeField] private float idleTime = 1f;

        private Vector3 lastDirection = Vector3.forward;
        private bool findingNewPos;
        #endregion
        
        [SerializeField, Self] private NavMeshAgent navMeshAgent;
        
        private Vector3 targetPosition;

        public override void OnUpdate()
        {
            base.OnUpdate();
            HandleIdleMovement();
            Move();
        }
        
        private void Move()
        {
            //Debug.Log("Move");
            navMeshAgent.SetDestination(targetPosition);
        }
        
        private void HandleIdleMovement()
        {
            if (inAggroRange || findingNewPos) return;

            bool shouldFindNew = navMeshAgent.velocity.sqrMagnitude < 1f;
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
            
            Vector3 gravity = LevelManager.Instance.PlayerController.PlayerMovement.Gravity.Value.normalized;
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
                    Debug.Log("Found new position");
                    targetPosition = hit.position;
                    lastDirection = (hit.position - transform.position).normalized;
                    return;
                }
            }
            
            findingNewPos = false;
        }

        public override void Unlock(IVisitor visitor)
        {
        }

        public override void UsePowerUp(InputAction.CallbackContext context)
        {
        }
        
    }
}