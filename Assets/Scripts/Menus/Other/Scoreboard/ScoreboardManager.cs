using System.Collections;
using LootLocker.Requests;
using RogueLike.Controllers;
using RogueLike.Timer;
using TMPro;
using UnityEngine;

namespace DeadLink.Menus.Other.Scoreboard
{
    public class ScoreboardManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text[] names, scores;
        [SerializeField] private TMP_Text playerScore;
        [SerializeField] private TMP_InputField playerName;
        
        private bool isDone;
        
        public void OnSubmitScore()
        {
            StartCoroutine(SubmitScore());
        }

        private IEnumerator StartSession()
        {
            isDone = false;
            bool done = false;
            string playerId = System.Guid.NewGuid().ToString();

            LootLockerSDKManager.StartGuestSession(playerId, (response) =>
            {
                if (!response.success)
                {
                    Debug.LogError("Error starting LootLocker session: " + response.errorData);
                    done = true;
                }
                else
                {
                    Debug.Log("Successfully started LootLocker session for player " + playerId);
                    done = true;
                }
            });
            
            yield return new WaitWhile(() => done == false);

            StartCoroutine(SetPlayerName());
        }
        
        private IEnumerator SetPlayerName()
        {
            bool done = false;
            
            LootLockerSDKManager.SetPlayerName(playerName.text, (response) =>
            {
                if (!response.success)
                {
                    Debug.LogError("Failed to set player name: " + response.errorData);
                    done = true;
                }
                else
                {
                    Debug.Log("Player name set successfully!");
                    done = true;
                }
            });

            yield return new WaitWhile(() => done == false);

            isDone = true;
        }
        
        private IEnumerator SubmitScore()
        {
            StartCoroutine(StartSession());
            yield return new WaitWhile(() => isDone == false);
            
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

            StartCoroutine(RefreshLeaderboard());
        }
        
        private IEnumerator RefreshLeaderboard()
        {
            StartCoroutine(StartSession());
            yield return new WaitWhile(() => isDone == false);
            
            bool done = false;
            string leaderboardKey = GameController.Metrics.LevelOneNormal;
            int count = scores.Length;

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

                        if (score.player.name != "")
                        {
                            names[index].text = score.player.name;
                        }
                        else
                        {
                            names[index].text = score.player.id.ToString();
                        }
                        scores[index].text = score.score.ToString();
                    }
                    
                    done = true;
                }
            });
            
            yield return new WaitWhile(() => done == false);
        }
    }
}