using RogueLike;
using UnityEngine;

namespace DeadLink.Menus
{
    [CreateAssetMenu(menuName = "RogueLike/Menus/MenuType", fileName = "MenuType")]
    public class MenuType : ScriptableObject
    {
        public static MenuType HUD => GameMetrics.Global.HUDMenu;
        public static MenuType Main => GameMetrics.Global.MainMenu;
        public static MenuType Pause => GameMetrics.Global.PauseMenu;
        public static MenuType Settings => GameMetrics.Global.SettingsMenu;
        public static MenuType Upgrades => GameMetrics.Global.UpgradesMenu;
        public static MenuType Credits => GameMetrics.Global.CreditsMenu;
        public static MenuType GameplayScoreboard => GameMetrics.Global.GameplayScoreboard;
        public static MenuType MenuScoreboard => GameMetrics.Global.MenuScoreboard;
        public static MenuType DieMenu => GameMetrics.Global.DieMenu;
        public static MenuType WinningMenu => GameMetrics.Global.WinningMenu;
    }
}