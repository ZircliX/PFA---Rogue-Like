using DG.Tweening;
using KBCore.Refs;
using UnityEngine;

namespace DeadLink.Menus.Implementation.Extensions
{
    public class ContextWindow : MonoBehaviour
    {
        [SerializeField] private bool baseState = false;
        [SerializeField, Self] private CanvasGroup canvasGroup;
        private bool currentState = false;
        
        private void OnValidate()
        {
            this.ValidateRefs();
            currentState = baseState;
            canvasGroup.alpha = currentState ? 1 : 0;
            ChangeState(true);
        }
        
        public void ChangeState(bool validate = false)
        {
            if (!validate)
                currentState = !currentState;
            
            canvasGroup.DOFade(currentState ? 1 : 0, 0.25f).SetUpdate(true);
        }
    }
}