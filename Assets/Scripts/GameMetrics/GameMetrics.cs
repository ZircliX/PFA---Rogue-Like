using DeadLink.Menus;
using DeadLink.PowerUpSystem;
using DevLocker.Utils;
using RogueLike.Controllers;
using UnityEngine;

namespace RogueLike
{
    [CreateAssetMenu(menuName = "RogueLike/GameMetrics")]
    public partial class GameMetrics : ScriptableObject
    {
        public static GameMetrics Global => GameController.Metrics;
        
        [field : SerializeField] public SceneLoader SceneLoader { get; private set; }
        
        [field: Header("Scenes")]
        [field: SerializeField] public SceneReference MainMenuScene { get; private set; }
        [field: SerializeField] public SceneReference ShopScene { get; private set; }
        [field: SerializeField] public SceneReference LevelOne { get; private set; }
        
        [field: Header("Menus")]
        [field: SerializeField] public MenuType HUD { get; private set; }
        [field: SerializeField] public MenuType Main { get; private set; }
        [field: SerializeField] public MenuType Pause { get; private set; }
        [field: SerializeField] public MenuType Settings { get; private set; }
        [field: SerializeField] public MenuType Upgrades { get; private set; }
        
        [field: Header("VisitableComponents")]
        
        [field: SerializeField] public VisitableType PlayerVisitableType { get; private set; }
        [field: SerializeField] public VisitableType PlayerMovementVisitableType { get; private set; }
    }
}