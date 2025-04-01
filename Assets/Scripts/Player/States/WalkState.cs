using UnityEngine;

namespace RogueLike.Player.States
{
    [CreateAssetMenu(menuName = "RogueLike/Movement/Walk")]
    public class WalkState : MoveState
    {
        public override MovementState State => MovementState.Walking;
    }
}