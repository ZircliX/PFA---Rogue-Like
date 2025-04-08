using UnityEngine;

namespace DeadLink.Cameras
{
    [System.Serializable]
    public struct CameraShakeComposite
    {
        public float Delay;
        public float Amplitude;
        public float Frequency;
        public float Duration;
        public AnimationCurve Curve;

        public static CameraShakeComposite GetDefault()
        {
            return new CameraShakeComposite
            {
                Delay = 0,
                Amplitude = 0,
                Frequency = 0,
                Duration = 0,
                Curve = AnimationCurve.Linear(0, 1, 1, 0),
            };
        }
    }
}