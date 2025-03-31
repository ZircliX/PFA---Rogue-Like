using FMODUnity;
using RogueLike.Controllers;
using UnityEngine;

namespace RogueLike
{
    [CreateAssetMenu(menuName = "BritBites/GameMetrics")]
    public class GameMetrics : ScriptableObject
    {
        public static GameMetrics Global => GameController.Metrics;
        
        [field: SerializeField]
        public EventReference test { get; private set; }
    }
}