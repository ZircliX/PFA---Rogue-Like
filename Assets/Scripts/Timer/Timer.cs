using DeadLink.Level.Interfaces;
using UnityEngine;

namespace RogueLike.Timer
{
    public class Timer : LevelElement
    {
        public float ElapsedTime { get; private set; }
        
        public void UpdateTimer()
        {
            ElapsedTime += Time.deltaTime;
        }

        public void ResetTimer()
        {
            ElapsedTime = 0;
        }
        
        internal override ILevelElementInfos Pull()
        {
            return new TimerElementInfos()
            {
                timer = ElapsedTime
            };
        }

        internal override void Push(ILevelElementInfos levelElementInfos)
        {
            if (levelElementInfos is TimerElementInfos timerElementInfos)
            {
                ElapsedTime = timerElementInfos.timer;
            }
        }
    }
}