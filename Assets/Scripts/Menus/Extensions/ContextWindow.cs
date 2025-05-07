using DG.Tweening;
using EditorAttributes;
using KBCore.Refs;
using UnityEngine;

namespace DeadLink.Menus.Implementation.Extensions
{
    public class ContextWindow : MonoBehaviour
    {
        [SerializeField, Self] private CanvasGroup canvasGroup;
        [ReadOnly, SerializeField] private bool currentState = false;
        
        private void OnValidate() => this.ValidateRefs();
        
        public void Enter()
        {
            currentState = true;
            ChangeCanvasAlpha();
        }

        public void Exit()
        {
            currentState = false;
            ChangeCanvasAlpha();
        }

        private void ChangeCanvasAlpha()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                canvasGroup.alpha = currentState ? 1 : 0;
                canvasGroup.blocksRaycasts = currentState;
                return;
            }
#endif
            
            canvasGroup.DOFade(currentState ? 1 : 0, 0.25f).SetUpdate(true);
            canvasGroup.blocksRaycasts = currentState;
        }
    }
}