using RogueLike;
using UnityEngine;

namespace DeadLink.Menus
{
    [CreateAssetMenu(menuName = "RogueLike/Menus/MenuType", fileName = "MenuType")]
    public class MenuType : ScriptableObject
    {
        public static MenuType HUD => GameMetrics.Global.HUD;
        public static MenuType Main => GameMetrics.Global.Main;
        public static MenuType Pause => GameMetrics.Global.Pause;
        public static MenuType Settings => GameMetrics.Global.Settings;
        public static MenuType Upgrades => GameMetrics.Global.Upgrades;
    }
}