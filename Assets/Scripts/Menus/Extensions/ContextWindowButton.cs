using System;
using DG.Tweening;
using EditorAttributes;
using KBCore.Refs;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DeadLink.Menus.Extensions
{
    public class ContextWindowButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private struct ButtonProperties
        {
            public Color color;
            public float size;

            public ButtonProperties(Color color, float size)
            {
                this.color = color;
                this.size = size;
            }
        }
        
        [Header("Settings")]
        [SerializeField] private float minFontSize = 24f;
        [SerializeField] private float hoverFontSize = 30f;
        [SerializeField] private float maxFontSize = 36f;

        [SerializeField] private Color minColor;
        [SerializeField] private Color hoverColor;
        [SerializeField] private Color maxColor;
        
        [Header("References")]
        [SerializeField] private Image underline;
        [SerializeField, Child] private TMP_Text text;
        [SerializeField, Self] private Button Button;

        [ReadOnly, SerializeField] private bool currentState = false;
        [ReadOnly, SerializeField] private bool hoverState = false;
        
        public event Action<ContextWindowButton> OnButtonClick; 
        
        private void OnValidate() => this.ValidateRefs();
        private void OnEnable() => Button.onClick.AddListener(ButtonClick);
        private void OnDisable() => Button.onClick.RemoveListener(ButtonClick);
        private void ButtonClick() => OnButtonClick?.Invoke(this);

        public void Enter()
        {
            currentState = true;
            hoverState = false;
            UpdateButton();
        }

        public void Exit()
        {
            currentState = false;
            hoverState = false;
            UpdateButton();
        }

        private void UpdateButton()
        {
            ButtonProperties properties = GetProperties();
            
            text.text = name;
            
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                text.fontSize = properties.size;
                text.color = properties.color;
                underline.color = properties.color;
                return;
            }
#endif
            
            text.DOKill();
            underline.DOKill();
            
            text.DOFontSize(properties.size, 0.25f).SetUpdate(true);
            text.DOColor(properties.color, 0.25f).SetUpdate(true);
            underline.DOColor(properties.color, 0.25f).SetUpdate(true);
        }
        
        private void HoverText()
        {
            UpdateButton();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (currentState) return;
            hoverState = true;
            HoverText();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (currentState) return;
            hoverState = false;
            HoverText();
        }

        private ButtonProperties GetProperties()
        {
            ButtonProperties properties;
            
            if (currentState)
            {
                properties = new ButtonProperties(maxColor, maxFontSize);
                return properties;
            }
            else if (hoverState)
            {
                properties = new ButtonProperties(hoverColor, hoverFontSize);
                return properties;
            }
            
            properties = new ButtonProperties(minColor, minFontSize);
            return properties;
        }
    }
}