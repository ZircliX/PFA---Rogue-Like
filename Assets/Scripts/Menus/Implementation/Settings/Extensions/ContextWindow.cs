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
        
        private void OnValidate()
        {
            this.ValidateRefs();
        }
        
        public void SetValues(bool value)
        {
            currentState = value;
            canvasGroup.alpha = currentState ? 1 : 0;
        }
        
        public void ChangeState(bool validate = false)
        {
            currentState = !validate ? !currentState : currentState;
            
            canvasGroup.DOFade(currentState ? 1 : 0, 0.25f).SetUpdate(true);
        }
    }
}