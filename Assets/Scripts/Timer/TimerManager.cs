using DeadLink.Level.Interfaces;
using LTX.Singletons;
using UnityEngine;

namespace RogueLike.Timer
{
    public class TimerManager : MonoSingleton<TimerManager>
    {
        [Header("Timer State")]
        [field: SerializeField] public Timer Timer { get; private set; }
        private bool isRunning;
        

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                StartTimer();
            }
            
            if (Input.GetKeyDown(KeyCode.N))
            {
                Debug.Log("PausedTimer Timer");
                PauseTimer();
            }
            
            if (Input.GetKeyDown(KeyCode.O))
            {
                ResetTimer();
            }
            
            //END DEBUG FUNCTIONS
            
            if (isRunning)
            {
                Timer.UpdateTimer();
            }
        }
        
        public void StartTimer()
        {
            if (!isRunning)
            {
                isRunning = true;
            }
        }

        public void PauseTimer()
        {
            if (isRunning)
            {
                isRunning = !isRunning;
            }
        }

        public void ResetTimer()
        {
            isRunning = false;
            Timer.ResetTimer();
        }
    }
}