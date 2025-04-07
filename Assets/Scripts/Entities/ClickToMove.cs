using KBCore.Refs;
using UnityEngine;
using UnityEngine.AI;

namespace DeadLink.Entities
{
    public class ClickToMove : MonoBehaviour {
        [SerializeField] private Transform player;
        [SerializeField, Self] private NavMeshAgent agent;

        private void OnValidate() => this.ValidateRefs();
        
        private void Update() 
        {
            agent.destination = player.position;
        }
    }
}