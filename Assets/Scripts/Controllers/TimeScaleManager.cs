using UnityEngine;

namespace RogueLike.Controllers
{
    public static class TimeScaleManager 
    {
        private static float SlowedTimeScale = 1f;

        public static float DeltaTimeScale { get; private set; } = SlowedTimeScale * Time.deltaTime;

        public static void SlowTimeScale()
        {
            SlowedTimeScale = 0.5f;
        }
        
        public static void ResetTimeScale()
        {
            SlowedTimeScale = 1f;
        }
    }
}