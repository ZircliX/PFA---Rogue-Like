using DeadLink.Menus.Other.Scoreboard;
using DeadLink.SceneManagement;
using LTX.ChanneledProperties;
using RogueLike;
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

        public void GoToShop()
        {
            SceneController.Global.ChangeScene(GameMetrics.Global.ShopScene);
        }

        public override void Open()
        {
            base.Open();
            ScoreboardViewer.Instance.RefreshScoreboard();
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