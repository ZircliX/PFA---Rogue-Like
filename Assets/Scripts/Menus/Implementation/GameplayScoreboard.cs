using DeadLink.Menus.Other.Scoreboard;
using DeadLink.SceneManagement;
using LTX.ChanneledProperties;
using RogueLike;
using RogueLike.Controllers;
using UnityEngine;

namespace DeadLink.Menus.Implementation
{
    public class GameplayScoreboard : Menu
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
                false,
                false);
        }
        
        private void Awake()
        {
            MenuType = MenuType.GameplayScoreboard;
        }
        
        public override void Open()
        {
            base.Open();
            ScoreboardUploader.Instance.OnSubmitScore();
        }

        public void GoToShop()
        {
            SceneController.Global.GoToNextLevel(gameObject.scene.buildIndex);
        }

        public void Back()
        {
            MenuManager.Instance.CloseMenu();
        }
    }
}