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
            StartCoroutine(ISubmitScore());
        }
        
        private IEnumerator ISubmitScore()
        {
            bool done = false;
            string leaderboardKey = GameController.Metrics.LevelOneNormal;
            
            LootLockerSDKManager.SubmitScore("", (int)TimerManager.Instance.ElapsedTime, leaderboardKey, (response) =>
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

            ScoreboardViewer.Instance.RefreshScoreboard();
        }
    }
}