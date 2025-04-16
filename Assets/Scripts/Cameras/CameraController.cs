using System.Collections;
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
        [SerializeField, Self] private CinemachineFollow camFollow;
        
        [Header("Camera Shake")]
        public PrioritisedProperty<CameraShakeComposite> CameraShakeProperty { get; private set; }

        private CameraShakeComposite currentComposite;
        private float currentShakeTime;
        private Vector3 originalOffset;
        private Coroutine shakeCoroutine;
        
        private void OnValidate() => this.ValidateRefs();

        protected override void Awake()
        {
            base.Awake();
            
            CameraShakeProperty = new PrioritisedProperty<CameraShakeComposite>(new CameraShakeComposite(0, 0, 0));
            CameraShakeProperty.AddOnValueChangeCallback(ShakeCamera, true);
            originalOffset = camFollow.FollowOffset;
        }

        protected override void OnDestroy()
        {
            CameraShakeProperty.RemovePriority(this);
            base.OnDestroy();
        }
        
        private IEnumerator ShakeRoutine()
        {
            float timer = 0f;
            float shakeInterval = 1f / Mathf.Max(currentComposite.Frequency, 0.01f);
            float shakeTimer = 0f;

            Vector3 randomOffset = Vector3.zero;

            while (timer < currentComposite.Duration)
            {
                timer += Time.deltaTime;
                shakeTimer += Time.deltaTime;

                //Debug.Log($"shake timer: {shakeTimer}, shakeInterval: {shakeInterval}");
                if (shakeTimer >= shakeInterval)
                {
                    shakeTimer = 0f;
                    randomOffset = Random.insideUnitSphere * currentComposite.Amplitude;
                    //Debug.Log($"Shake camera with offset: {randomOffset}");
                }

                camFollow.FollowOffset = originalOffset + randomOffset;

                yield return null;
            }

            camFollow.FollowOffset = originalOffset;
        }
        
        private void ShakeCamera(CameraShakeComposite composite)
        {
            currentShakeTime = composite.Duration;
            currentComposite = composite;

            if (shakeCoroutine != null)
            {
                StopCoroutine(shakeCoroutine);
            }

            shakeCoroutine = StartCoroutine(ShakeRoutine());
            Debug.Log($"Started camera shake with composite: {composite}");
        }
    }
}