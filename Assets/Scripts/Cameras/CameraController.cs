using System.Collections;
using System.Data;
using DG.Tweening;
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
        [SerializeField, Self] private CinemachineRecomposer camRecomposer;
        
        [Header("Camera Properties")]
        public PrioritisedProperty<CameraShakeComposite> CameraShakeProperty { get; private set; }
        public PrioritisedProperty<CameraEffectComposite> CameraEffectProperty { get; private set; }

        private CameraShakeComposite currentShakeComposite;
        private Coroutine shakeCoroutine;
        private Vector3 originalOffset;
        private CameraEffectComposite currentEffectComposite;
        private Coroutine effectCoroutine;
        
        private void OnValidate() => this.ValidateRefs();

        protected override void Awake()
        {
            base.Awake();
            
            CameraShakeProperty = new PrioritisedProperty<CameraShakeComposite>();
            CameraShakeProperty.AddOnValueChangeCallback(ApplyCameraShake, true);

            CameraEffectProperty = new PrioritisedProperty<CameraEffectComposite>(CameraEffectComposite.Default);
            CameraEffectProperty.AddOnValueChangeCallback(ApplyCameraEffect, true);
            
            originalOffset = camFollow.Offset;
        }
        
        private IEnumerator IShakeRoutine()
        {
            float timer = 0f;
            float shakeInterval = 1f / Mathf.Max(currentShakeComposite.Frequency, 0.01f);
            float shakeTimer = 0f;

            Vector3 targetOffset = Vector3.zero;
            Vector3 currentOffset = Vector3.zero;

            while (timer < currentShakeComposite.Duration)
            {
                timer += Time.deltaTime;
                shakeTimer += Time.deltaTime;

                //Debug.Log($"shake timer: {shakeTimer}, shakeInterval: {shakeInterval}");
                if (shakeTimer >= shakeInterval)
                {
                    shakeTimer = 0f;
                    targetOffset = Random.insideUnitSphere * currentShakeComposite.Amplitude;
                }

                currentOffset = Vector3.Lerp(currentOffset, targetOffset, 0.5f);
                camFollow.Offset = originalOffset + currentOffset;
                
                yield return null;
            }

            camFollow.Offset = originalOffset;
        }
        
        private void ApplyCameraShake(CameraShakeComposite composite)
        {
            if (!camFollow.isActiveAndEnabled) return;
            
            currentShakeComposite = composite;

            if (shakeCoroutine != null)
            {
                StopCoroutine(shakeCoroutine);
            }

            shakeCoroutine = StartCoroutine(IShakeRoutine());
            //Debug.Log($"Started camera shake with composite: {composite}");
        }

        private IEnumerator IEffectCoroutine()
        {
            DOTween.To(
                () => camRecomposer.Dutch,
                value => camRecomposer.Dutch = value,
                CameraEffectProperty.Value.Dutch,
                CameraEffectProperty.Value.Speed
            );
            DOTween.To(
                () => camRecomposer.ZoomScale,
                value => camRecomposer.ZoomScale = value,
                CameraEffectProperty.Value.FovScale,
                CameraEffectProperty.Value.Speed
            );

            yield return null;
        }

        private void ApplyCameraEffect(CameraEffectComposite composite)
        {
            if (!camRecomposer.isActiveAndEnabled) return;
            
            currentEffectComposite = composite;
            
            if (effectCoroutine != null)
            {
                StopCoroutine(effectCoroutine);
            }
            
            effectCoroutine = StartCoroutine(IEffectCoroutine());
        }
    }
}