using KBCore.Refs;
using RogueLike.Managers;
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
            if (player == null)
            {
                player = LevelManager.Instance.player.transform;
            }
            
            agent.destination = player.position;
        }
    }
}