using UnityEngine;
using TMPro;
using System;
using LTX.Singletons;

namespace RogueLike.Controllers
{
    public class TimerController : MonoSingleton<TimerController>
    {
        [Header("UI Reference")]
        [SerializeField]
        private TextMeshProUGUI timerText;

        [Header("Timer State")]
        private float elapsedTime;
        private bool isRunning;

        private void Awake()
        {
            if (timerText == null)
            {
                Debug.LogError("Timer Text (TextMeshProUGUI) n'est pas assigné dans l'inspecteur !");
                enabled = false;
                return;
            }
            UpdateTimerDisplay();
        }

        public void Update()
        {
            //DEBUG FUNCTIONS :

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
                elapsedTime += Time.deltaTime;
                UpdateTimerDisplay();
            }
        }

        private void UpdateTimerDisplay()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(elapsedTime);
            string timeString = timeSpan.ToString(@"hh\:mm\:ss");
            timerText.text = timeString;
        }

        public void StartTimer()
        {
            if (!isRunning)
            {
                isRunning = true;
                Debug.Log("Timer Started/Resumed.");
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
            elapsedTime = 0f;
            UpdateTimerDisplay();
        }
    }
}