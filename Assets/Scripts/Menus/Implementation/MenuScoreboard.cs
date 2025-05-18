using DeadLink.Menus.Other.Scoreboard;
using LTX.ChanneledProperties;
using UnityEngine;

namespace DeadLink.Menus.Implementation
{
    public class MenuScoreboard : Menu
    {
        public override MenuType MenuType { get; protected set; }
        private string levelIndex = "1";
        private string difficultyIndex = "Easy";
        
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
            MenuType = MenuType.GameplayScoreboard;
        }
        
        public override void Open()
        {
            base.Open();
            RefreshScoreboard();
        }

        public void Back()
        {
            MenuManager.Instance.CloseMenu();
        }
        
        public void ChangeDifficulty(string difficulty)
        {
            difficultyIndex = difficulty;
            RefreshScoreboard();
        }
        
        public void ChangeLevel(string level)
        {
            levelIndex = level;
            RefreshScoreboard();
        }

        private void RefreshScoreboard()
        {
            ScoreboardViewer.Instance.RefreshScoreboard($"{levelIndex}{difficultyIndex}");
        }
    }
}