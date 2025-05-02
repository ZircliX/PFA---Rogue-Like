using System;
using DG.Tweening;
using EditorAttributes;
using KBCore.Refs;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DeadLink.Menus.Implementation.Extensions
{
    public class ContextWindowButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float textSize;
        [SerializeField] private Image underline;
        [SerializeField, Child] private TMP_Text text;
        [SerializeField, Self] private Button Button;

        private string title => name;
        [ReadOnly, SerializeField] private bool currentState = false;
        
        public event Action<ContextWindowButton> OnButtonClick; 
        
        private void OnValidate()
        {
            this.ValidateRefs();
        }

        public void SetValues(bool value)
        {
            currentState = value;
            
            (Color, float) attributes = GetAttributes(currentState, textSize * 1.5f);
            text.text = title;
            
            text.fontSize = attributes.Item2;
            text.color = attributes.Item1;
            underline.color = attributes.Item1;
        }

        private void OnEnable() => Button.onClick.AddListener(ButtonClick);
        private void OnDisable() => Button.onClick.RemoveListener(ButtonClick);
        private void ButtonClick() => OnButtonClick?.Invoke(this);

        public void ChangeState(bool validate = false)
        {
            currentState = !validate ? !currentState : currentState;
            ActivateText();
        }

        private (Color, float) GetAttributes(bool target, float maxSize, float maxAlpha = 1f)
        {
            float size = target ? maxSize : textSize;
            Color color = target ? new Color(0, 0, 0, maxAlpha) : new Color(0, 0, 0, 0.6f);

            return (color, size);
        }

        private void ActivateText()
        {
            (Color, float) attributes = GetAttributes(currentState, textSize * 1.5f);
            
            text.DOKill();
            text.text = title;
            
            text.DOFontSize(attributes.Item2, 0.25f).SetUpdate(true);
            text.DOColor(attributes.Item1, 0.25f).SetUpdate(true);
            underline.DOColor(attributes.Item1, 0.25f).SetUpdate(true);
        }
        
        private void HoverText(bool hover)
        {
            (Color, float) attributes = GetAttributes(hover, textSize * 1.25f, 0.8f);
            
            text.DOKill();
            
            text.DOFontSize(attributes.Item2, 0.25f).SetUpdate(true);
            text.DOColor(attributes.Item1, 0.25f).SetUpdate(true);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (currentState) return;
            HoverText(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (currentState) return;
            HoverText(false);
        }
    }
}