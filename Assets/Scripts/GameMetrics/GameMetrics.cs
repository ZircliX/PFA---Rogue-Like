using DevLocker.Utils;
using RogueLike.Controllers;
using UnityEngine;

namespace RogueLike
{
    [CreateAssetMenu(menuName = "RogueLike/GameMetrics")]
    public partial class GameMetrics : ScriptableObject
    {
        public static GameMetrics Global => GameController.Metrics;
        
        [field: Header("Scenes")]
        [field: SerializeField] public SceneReference MainMenuScene { get; private set; }
        [field: SerializeField] public SceneReference LevelScene { get; private set; }
    }
}