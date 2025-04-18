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
        [field: SerializeField] public MenuType HUDMenu { get; private set; }
        [field: SerializeField] public MenuType MainMenu { get; private set; }
        [field: SerializeField] public MenuType PauseMenu { get; private set; }
        [field: SerializeField] public MenuType SettingsMenu { get; private set; }
        [field: SerializeField] public MenuType UpgradesMenu { get; private set; }
        
        [field: Header("VisitableComponents")]
        
        [field: SerializeField] public VisitableType PlayerVisitableType { get; private set; }
        [field: SerializeField] public VisitableType PlayerMovementVisitableType { get; private set; }
    }
}