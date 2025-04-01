using UnityEngine;

namespace RogueLike.Player.States
{
    [CreateAssetMenu(menuName = "RogueLike/Movement/Fall")]
    public class FallState : MoveState
    {
        public override MovementState State => MovementState.Falling;
    }
}