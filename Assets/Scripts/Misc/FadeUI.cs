using System.Collections;
using DG.Tweening;
using LTX.Singletons;
using UnityEngine;

namespace DeadLink.Misc
{
    public class FadeUI : MonoSingleton<FadeUI>
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private bool baseFade = true;

        protected override void Awake()
        {
            base.Awake();
            if (baseFade) canvasGroup.alpha = 1;
        }

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            if (baseFade) FadeOut(1f);
        }

        /// <summary>
        /// Used to fade the screen toward black
        /// </summary>
        /// <param name="duration"></param>
        public Tweener FadeIn(float duration)
        {
            //fadeImage.DOKill();
            return canvasGroup.DOFade(1f, duration).SetUpdate(true).SetEase(Ease.Linear);
        }
        
        /// <summary>
        /// Used to fade the screen toward white
        /// </summary>
        /// <param name="duration"></param>
        public Tweener FadeOut(float duration)
        {
            //fadeImage.DOKill();
            return canvasGroup.DOFade(0f, duration).SetUpdate(true).SetEase(Ease.Linear);
        }
    }
}