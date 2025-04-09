using KBCore.Refs;
using LTX.ChanneledProperties;
using UnityEngine;

namespace DeadLink.Gravity
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyGravityReceiver : MonoBehaviour, IGravityReceiver
    {
        [SerializeField, Self] private Rigidbody rb;
        
        public Vector3 Position => rb.position;
        public PrioritisedProperty<Vector3> Gravity { get; private set; }
        
        private void OnValidate()
        {
            this.ValidateRefs();
        }
        
        private void Awake()
        {
            Gravity = new PrioritisedProperty<Vector3>(Vector3.zero);
        }

        private void FixedUpdate()
        {
            rb.AddForce(Gravity, ForceMode.Force);
        }
    }
}