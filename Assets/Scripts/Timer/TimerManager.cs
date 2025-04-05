using UnityEngine;
using LTX.Singletons;

namespace RogueLike.Timer
{
    public class TimerManager : MonoSingleton<TimerManager>
    {
        [Header("Timer State")]
        private bool isRunning;
        public float ElapsedTime { get; private set; }

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
                ElapsedTime += Time.deltaTime;
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
            ElapsedTime = 0f;
        }
    }
}