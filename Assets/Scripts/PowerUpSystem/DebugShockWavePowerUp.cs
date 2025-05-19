using System;
using DeadLink.Entities.Enemies;
using UnityEngine;
using UnityEngine.AI;

namespace DeadLink.PowerUpSystem
{
    public class DebugShockWavePowerUp : MonoBehaviour
    {
        [field : SerializeField] public float radius { get; private set; } = 5f;
        [field : SerializeField] public float pushDistance { get; private set; } = 3f;
        public LayerMask enemyLayer;
        private void Start()
        {

            Explode();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                Explode();
            }
        }

        private void Explode()
        {
            Collider[] enemies = Physics.OverlapSphere(transform.position, radius, enemyLayer);
            Debug.Log(enemies.Length);
            foreach (Collider enemyCollider in enemies)
            {
                var enemy = enemyCollider.GetComponent<MeleeEnemy>();
                if (enemy != null)
                {
                    NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
                    if (agent != null)
                    {
                        Vector3 pushDirection = (enemy.transform.position - transform.position).normalized;
                        Vector3 newTargetPos = enemy.transform.position + pushDirection * pushDistance;

                        if (NavMesh.SamplePosition(newTargetPos, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                        {
                            agent.SetDestination(hit.position);
                        }
                    }
                }
            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, radius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, Vector3.up * 1f);
        }

    }
}