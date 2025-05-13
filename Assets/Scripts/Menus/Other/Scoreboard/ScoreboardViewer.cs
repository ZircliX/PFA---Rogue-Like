using System.Collections;
using LootLocker.Requests;
using LTX.Singletons;
using RogueLike.Controllers;
using UnityEngine;

namespace DeadLink.Menus.Other.Scoreboard
{
    public class ScoreboardViewer : MonoSingleton<ScoreboardViewer>
    {
        [SerializeField] private ScoreField[] scoreFields;
        
        private bool isDone;

        public void RefreshScoreboard()
        {
            StartCoroutine(IRefreshLeaderboard());
        }
        
        private IEnumerator IRefreshLeaderboard()
        {
            bool done = false;
            string leaderboardKey = GameController.Metrics.LevelOneNormal;
            int count = scoreFields.Length;

            LootLockerSDKManager.GetScoreList(leaderboardKey, count, 0, (response) =>
            {
                if (!response.success)
                {
                    Debug.LogError("Could not get score list: " + response.errorData);
                    done = true;
                }
                else
                {
                    Debug.Log("Successfully got score list!");
                    
                    for (int index = 0; index < response.items.Length; index++)
                    {
                        LootLockerLeaderboardMember score = response.items[index];
                        string playerName = score.player.name != "" ? score.player.name : score.player.id.ToString();
                        
                        scoreFields[index].SetScore(playerName, score.score);
                    }
                    
                    done = true;
                }
            });
            
            yield return new WaitWhile(() => done == false);
        }
    }
}