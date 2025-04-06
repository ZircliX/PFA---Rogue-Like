using System;
using TMPro;
using UnityEngine;

namespace RogueLike.Timer
{
    public class TimerUI : MonoBehaviour
    {
        [Header("UI Reference")]
        [SerializeField] private TMP_Text timerText;

        private void Update()
        {
            UpdateTimer();
        }

        private void UpdateTimer()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(TimerManager.Instance.ElapsedTime);
            string timeString = timeSpan.ToString(@"hh\:mm\:ss");
            timerText.text = timeString;
        }
    }
}