using UnityEngine;

namespace RogueLike.Player.States
{
    [CreateAssetMenu(menuName = "RogueLike/Movement/Run")]
    public class RunState : WalkState
    {
        public override MovementState State => MovementState.Running;
    }
}