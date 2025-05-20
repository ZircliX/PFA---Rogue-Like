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

        public void RefreshScoreboard(string leaderboardKey = "1Easy")
        {
            StartCoroutine(IRefreshLeaderboard(leaderboardKey));
        }
        
        private IEnumerator IRefreshLeaderboard(string leaderboardKey)
        {
            bool done = false;
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
                    
                    if (response.items != null)
                    {
                        for (int index = 0; index < response.items.Length; index++)
                        {
                            LootLockerLeaderboardMember score = response.items[index];
                            string playerName = score.player.name != "" ? score.player.name : score.player.id.ToString();
                            
                            scoreFields[index].SetScore(playerName, score.score);
                        }
                    }
                    else
                    {
                        for (int index = 0; index < scoreFields.Length; index++)
                        {
                            string playerName = "PLAYER";
                            
                            scoreFields[index].SetScore(playerName, 0);
                        }
                    }
                    
                    done = true;
                }
            });
            
            yield return new WaitWhile(() => done == false);
        }
    }
}