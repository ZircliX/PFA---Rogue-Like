using KBCore.Refs;
using UnityEngine;

namespace DeadLink.Gravity.Implementations
{
    [RequireComponent(typeof(SphereCollider)), AddComponentMenu("RogueLike/Gravity/Sphere")]
    public class SphereGravityZone : GravityZone
    {
        [SerializeField, Self] private SphereCollider sc;

        private void OnValidate()
        {
            this.ValidateRefs();
        }

        protected override Vector3 GetGravityForReceiver(IGravityReceiver receiver)
        {
            return transform.position + sc.center - receiver.Position;
        }
    }
}