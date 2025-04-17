using System.Collections.Generic;
using DeadLink.PowerUpSystem.InterfacePowerUps;
using LTX.Singletons;
using UnityEngine;

namespace DeadLink.PowerUpSystem
{
    public class VisitableReferenceManager : MonoSingleton<VisitableReferenceManager>
    {
        private Dictionary<VisitableType, IVisitable> Components;

        protected override void Awake()
        {
            base.Awake();
            
            Components = new Dictionary<VisitableType, IVisitable>();
        }
        
        public void RegisterComponent(VisitableType type, IVisitable component)
        {
            if (!Components.TryAdd(type, component))
            {
                Debug.LogWarning($"Component of type {type} is already registered.");
            }
        }
        
        public void UnregisterComponent(VisitableType type)
        {
            if (!Components.Remove(type))
            {
                Debug.LogWarning($"Component of type {type} is not registered.");
            }
        }
        
        public bool TryGetComponent(VisitableType type, out IVisitable component) => Components.TryGetValue(type, out component);
        
        
    }
}