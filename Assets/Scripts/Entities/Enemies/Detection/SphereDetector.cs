using System;
using KBCore.Refs;
using UnityEngine;

namespace DeadLink.Entities.Enemies.Detection
{
    [RequireComponent(typeof(SphereCollider))]
    public class SphereDetector : MonoBehaviour
    {
        [field: SerializeField, Self] public SphereCollider SphereCollider { get; private set; }
        
        public event Action<Collider, SphereDetector> OnTriggerEnterEvent;
        public event Action<Collider, SphereDetector> OnTriggerStayEvent;
        public event Action<Collider, SphereDetector> OnTriggerExitEvent;
        
        private void OnValidate() => this.ValidateRefs();

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterEvent?.Invoke(other, this);
        }

        private void OnTriggerStay(Collider other)
        {
            OnTriggerStayEvent?.Invoke(other, this);
        }

        private void OnTriggerExit(Collider other)
        {
            OnTriggerExitEvent?.Invoke(other, this);
        }
    }
}