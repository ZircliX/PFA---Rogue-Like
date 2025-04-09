using KBCore.Refs;
using LTX.ChanneledProperties;
using LTX.Singletons;
using Unity.Cinemachine;
using UnityEngine;

namespace DeadLink.Cameras
{
    public class CameraController : MonoSingleton<CameraController>
    {
        [Header("References")]
        [SerializeField, Self] private CinemachineCamera cam;
        [SerializeField, Self] private CinemachineBasicMultiChannelPerlin noise;
        
        [Header("Camera Shake")]
        public PrioritisedProperty<CameraShakeComposite> CameraShakeProperty { get; private set; }

        private CameraShakeComposite currentComposite;
        private float currentShakeTime;
        private float currentDelayTime;
        
        private void OnValidate() => this.ValidateRefs();

        protected override void Awake()
        {
            base.Awake();
            
            CameraShakeProperty = new PrioritisedProperty<CameraShakeComposite>(CameraShakeComposite.GetDefault());
            CameraShakeProperty.AddOnValueChangeCallback(ShakeCamera, true);
        }
        
        private void Update()
        {
            //Manage Timers
            if (currentDelayTime > 0)
            {
                currentDelayTime -= Time.deltaTime;
            }
            else
            {
                currentShakeTime -= Time.deltaTime;
            }
            
            //Manage Shake
            if (currentShakeTime > 0 && currentDelayTime <= 0)
            {
                float lerp = currentComposite.Curve.Evaluate(currentShakeTime - currentComposite.Duration);
                noise.AmplitudeGain = Mathf.Lerp(currentComposite.Amplitude, 0, lerp);
            }
            else
            {
                noise.AmplitudeGain = 0;
            }
        }
        
        /// <summary>
        /// Shakes the camera based on the provided composite.
        /// </summary>
        /// <param name="composite"></param>
        private void ShakeCamera(CameraShakeComposite composite)
        {
            noise.AmplitudeGain = composite.Amplitude;
            noise.FrequencyGain = composite.Frequency;

            currentShakeTime = composite.Duration;
            currentDelayTime = composite.Delay;
            
            currentComposite = composite;
        }
    }
}
