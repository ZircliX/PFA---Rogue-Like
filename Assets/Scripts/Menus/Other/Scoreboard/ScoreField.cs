using TMPro;
using UnityEngine;

namespace DeadLink.Menus.Other.Scoreboard
{
    public class ScoreField : MonoBehaviour
    {
        [field: SerializeField] public TMP_Text PlayerName { get; private set; }
        [field: SerializeField] public TMP_Text Score { get; private set; }
        
        public void SetScore(string playerName, int score)
        {
            PlayerName.text = playerName;
            Score.text = $"{(int)score / 60}:{score%60:00}";
        }
    }
}