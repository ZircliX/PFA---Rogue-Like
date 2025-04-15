using System;
using RogueLike.Controllers;
using LTX.Singletons;
using UnityEngine;

namespace RogueLike.Events
{
    public class EventManager : MonoSingleton<EventManager>
    {
        //Games States
        public event Action OnGameStart;
        public void GameStart()
        {
            Time.timeScale = 1;
            OnGameStart?.Invoke();
            GameController.Play();
        }

        public event Action OnGameEnd;
        public void GameEnd()
        {
            Time.timeScale = 0;
            OnGameEnd?.Invoke();
            GameController.End();
        }


        public event Action<int> OnGoldEarn;
        public void GoldEarn(int value)
        {
            GameController.SaveListener.AddCurrency(value);
            OnGoldEarn?.Invoke(value);
        }
    }
}