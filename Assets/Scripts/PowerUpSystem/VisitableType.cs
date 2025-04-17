using RogueLike;
using UnityEngine;

namespace DeadLink.PowerUpSystem
{
    [CreateAssetMenu(menuName = "PowerUp/VisitableType", fileName = "VisitableType")]
    public class VisitableType : ScriptableObject
    {
        public static VisitableType Player => GameMetrics.Global.PlayerVisitableType;
        public static VisitableType PlayerMovement => GameMetrics.Global.PlayerMovementVisitableType;
    }
}