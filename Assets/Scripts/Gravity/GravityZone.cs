using System.Collections.Generic;
using UnityEngine;

namespace DeadLink.Gravity
{
    [RequireComponent(typeof(Collider))]
    public abstract class GravityZone : MonoBehaviour
    {
        [SerializeField] private float gravityStrength = 9.81f;
        [SerializeField, Range(10, 50)] private int priority;
        private List<IGravityReceiver> gravityReceivers;
        
        protected virtual void OnEnable()
        {
            GravityManager.Instance.RegisterGravityZone(this);
        }

        protected virtual void OnDisable()
        {
            GravityManager.Instance.UnregisterGravityZone(this);
        }

        protected void Awake()
        {
            gravityReceivers = new List<IGravityReceiver>();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IGravityReceiver gravityReceiver))
            {
                Debug.Log($"Entered {other.name} into {name}");
                gravityReceivers.Add(gravityReceiver);
                gravityReceiver.Gravity.AddPriority(this, priority);
            }
        }
        
        protected virtual void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IGravityReceiver gravityReceiver))
            {
                Debug.Log($"Exited {other.name} into {name}");
                gravityReceivers.Remove(gravityReceiver);
                gravityReceiver.Gravity.RemovePriority(this);
            }
        }
        
        internal virtual void OnFixedUpdate()
        {
            foreach (IGravityReceiver receiver in gravityReceivers)
            {
                Vector3 gravityForReceiver = GetGravityForReceiver(receiver).normalized;
                receiver.Gravity.Write(this, gravityForReceiver * gravityStrength);
            }
        }
        
        protected abstract Vector3 GetGravityForReceiver(IGravityReceiver receiver);
    }
}