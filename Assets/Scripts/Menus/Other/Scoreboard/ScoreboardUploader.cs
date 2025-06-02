using System.Collections;
using LootLocker.Requests;
using LTX.Singletons;
using RogueLike.Controllers;
using RogueLike.Timer;
using UnityEngine;

namespace DeadLink.Menus.Other.Scoreboard
{
    public class ScoreboardUploader : MonoSingleton<ScoreboardUploader>
    {
        private bool isDone;
        
        public void OnSubmitScore()
        {
            string leaderboardKey = GameDatabase.Global.GetScoreboardKey();
            StartCoroutine(ISubmitScore(leaderboardKey));
        }
        
        private IEnumerator ISubmitScore(string leaderboardKey)
        {
            bool done = false;
            
            LootLockerSDKManager.SubmitScore("", (int)TimerManager.Instance.Timer.ElapsedTime, leaderboardKey, (response) =>
            {
                if (!response.success)
                {
                    Debug.LogError("Could not submit score: " + response.errorData);
                    done = true;
                }
                else
                {
                    Debug.Log("Successfully submitted score!");
                    done = true;
                }
            });
            
            yield return new WaitWhile(() => done == false);

            ScoreboardViewer.Instance.RefreshScoreboard(GameDatabase.Global.GetScoreboardKey());
        }
    }
}