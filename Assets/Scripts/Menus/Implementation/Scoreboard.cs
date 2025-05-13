using LTX.ChanneledProperties;
using UnityEngine;

namespace DeadLink.Menus.Implementation
{
    public class Scoreboard : Menu
    {
        public override MenuType MenuType { get; protected set; }
        
        public override MenuProperties GetMenuProperties()
        {
            return new MenuProperties(
                gameObject,
                PriorityTags.Default,
                0f,
                CursorLockMode.None,
                true,
                true,
                false);
        }
        
        private void Awake()
        {
            MenuType = MenuType.Scoreboard;
        }

        public void Back()
        {
            MenuManager.Instance.CloseMenu();
        }

        public void ChangeDifficulty()
        {
            
        }
        
        public void ChangeLevel()
        {
            
        }
    }
}