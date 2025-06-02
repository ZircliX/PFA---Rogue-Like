using System.Collections.Generic;
using DeadLink.Menus;
using DeadLink.SceneManagement;
using DevLocker.Utils;
using Enemy;
using RogueLike.Controllers;
using UnityEngine;

namespace RogueLike
{
    [CreateAssetMenu(menuName = "RogueLike/GameMetrics")]
    public partial class GameMetrics : ScriptableObject
    {
        public static GameMetrics Global => GameController.Metrics;
        
        [field: Header("Difficulties")]
        [field: SerializeField] public DifficultyData EasyDifficulty { get; private set; }
        [field: SerializeField] public DifficultyData NormalDifficulty { get; private set; }
        [field: SerializeField] public DifficultyData HardDifficulty { get; private set; }
        [field: SerializeField] public DifficultyData InsaneDifficulty { get; private set; }
        
        
        [field: Header("Debug Settings")]
        [field: SerializeField] public bool SpawnEnemies { get; private set; }
        [field : SerializeField] public LayerMask EnemyStopDetect { get; private set; }
        [field : SerializeField] public LayerMask BulletRayCast { get; private set; }
        
        #region Scenes
        [field: Header("Scenes")]
        [field: SerializeField] public SceneReference MainMenuScene { get; private set; }
        [field: SerializeField] public SceneReference ShopScene { get; private set; }
        [field: SerializeField] public SceneReference LevelOne { get; private set; }
        #endregion
        
        #region Menus
        [field: Header("Menus")]
        [field: SerializeField] public MenuType HUDMenu { get; private set; }
        [field: SerializeField] public MenuType MainMenu { get; private set; }
        [field: SerializeField] public MenuType PauseMenu { get; private set; }
        [field: SerializeField] public MenuType SettingsMenu { get; private set; }
        [field: SerializeField] public MenuType UpgradesMenu { get; private set; }
        [field: SerializeField] public MenuType CreditsMenu { get; private set; }
        [field: SerializeField] public MenuType GameplayScoreboard { get; private set; }
        [field: SerializeField] public MenuType MenuScoreboard { get; private set; }
        [field: SerializeField] public MenuType DieMenu { get; private set; }
        [field: SerializeField] public MenuType WinningMenu { get; private set; }
        #endregion
        
    }
}