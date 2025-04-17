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
        [SerializeField, Self] private CinemachineCameraOffset camFollow;
        
        [Header("Camera Shake")]
        public PrioritisedProperty<CameraShakeComposite> CameraShakeProperty { get; private set; }

        private CameraShakeComposite currentComposite;
        private Vector3 originalOffset;
        private Coroutine shakeCoroutine;
        
        private void OnValidate() => this.ValidateRefs();

        protected override void Awake()
        {
            base.Awake();
            
            CameraShakeProperty = new PrioritisedProperty<CameraShakeComposite>();
            CameraShakeProperty.AddOnValueChangeCallback(ShakeCamera, true);
            
            originalOffset = camFollow.Offset;
        }
        
        private IEnumerator ShakeRoutine()
        {
            float timer = 0f;
            float shakeInterval = 1f / Mathf.Max(currentComposite.Frequency, 0.01f);
            float shakeTimer = 0f;

            Vector3 targetOffset = Vector3.zero;
            Vector3 currentOffset = Vector3.zero;

            while (timer < currentComposite.Duration)
            {
                timer += Time.deltaTime;
                shakeTimer += Time.deltaTime;

                //Debug.Log($"shake timer: {shakeTimer}, shakeInterval: {shakeInterval}");
                if (shakeTimer >= shakeInterval)
                {
                    shakeTimer = 0f;
                    targetOffset = Random.insideUnitSphere * currentComposite.Amplitude;
                }

                currentOffset = Vector3.Lerp(currentOffset, targetOffset, 0.5f);
                camFollow.Offset = originalOffset + currentOffset;
                
                yield return null;
            }

            camFollow.Offset = originalOffset;
        }
        
        private void ShakeCamera(CameraShakeComposite composite)
        {
            if (!camFollow.isActiveAndEnabled) return;
            
            currentComposite = composite;

            if (shakeCoroutine != null)
            {
                StopCoroutine(shakeCoroutine);
            }

            shakeCoroutine = StartCoroutine(ShakeRoutine());
            //Debug.Log($"Started camera shake with composite: {composite}");
        }
    }
}