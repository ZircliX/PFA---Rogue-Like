using System.Collections;
using LootLocker.Requests;
using LTX.Singletons;
using RogueLike.Controllers;
using UnityEngine;

namespace DeadLink.Menus.Other.Scoreboard
{
    public class ScoreboardSession : MonoSingleton<ScoreboardSession>
    {
        private bool isDone;

        public void StartSession()
        {
            StartCoroutine(IStartSession());
        }
        
        private IEnumerator IStartSession()
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

            StartCoroutine(ISetPlayerName());
        }
        
        private IEnumerator ISetPlayerName()
        {
            bool done = false;
            
            LootLockerSDKManager.SetPlayerName(GameController.GameProgressionListener.PlayerName, (response) =>
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
    }
}