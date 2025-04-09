using DeadLink.Gravity;
using KBCore.Refs;
using LTX.ChanneledProperties;
using RogueLike.Player;
using UnityEngine;

namespace DeadLink
{
    public class PlayerGravity : GravityReceiver
    {
        [SerializeField, Parent] private PlayerMovement pm;
        
        private void OnValidate()
        {
            this.ValidateRefs();
        }

        public override Vector3 Position => pm.Position;

        protected override void Awake()
        {
            base.Awake();
            Gravity.AddOnValueChangeCallback(ctx => pm.Gravity.Write(this, GetGravityDirection()));
        }

        private void Start()
        {
            pm.Gravity.AddPriority(this, PriorityTags.Default, GetGravityDirection());
        }
    }
}
