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
        #endregion
        
        #region PowerUps
        [field: Header("PowerUps")]
        [field: SerializeField] public PowerUp[] PowerUps { get; private set; }
        /*
        [field: SerializeField] public PowerUp InstantHealPowerUp { get; private set; }
        [field: SerializeField] public PowerUp WallHackPowerUp { get; private set; }
        
        [field: SerializeField] public PowerUp SlowMotionPowerUp { get; private set; }
        
        [field: SerializeField] public PowerUp GrapplingHookPowerUp { get; private set; }
        
        [field: SerializeField] public PowerUp InvisibilityPowerUp { get; private set; }
        
        [field: SerializeField] public PowerUp ShockWavePowerUp { get; private set; }
        
        [field: SerializeField] public PowerUp FastFallPowerUp { get; private set; }
        
        [field: SerializeField] public PowerUp AdrenalineShotPowerUp { get; private set; }
        
        [field: SerializeField] public PowerUp ContinuousFirePowerUp { get; private set; }
        */
        
        public PowerUp GetPowerUp(string targetName)
        {
            for (int index = 0; index < PowerUps.Length; index++)
            {
                PowerUp powerUp = PowerUps[index];
                if (powerUp.Name == targetName)
                {
                    return powerUp;
                }
            }

            Debug.LogError($"PowerUp with name {targetName} not found.");
            return null;
        }
        #endregion
    }
}