using System;
using KBCore.Refs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DeadLink.Menus.Implementation.Extensions
{
    public class ContextWindowButton : MonoBehaviour
    {
        [SerializeField] private float textSize;
        [SerializeField] private Image image;
        [SerializeField, Child] private TMP_Text text;
        [field: SerializeField, Self] public Button Button { get; private set; }
        [field: SerializeField] public bool BaseState { get; private set; } = false;

        private string title => name;
        private bool currentState = false;
        
        public event Action<ContextWindowButton> OnButtonClick; 
        
        private void OnValidate()
        {
            this.ValidateRefs();

            currentState = BaseState;
            ChangeState(true);
        }

        private void OnEnable() => Button.onClick.AddListener(ButtonClick);
        private void OnDisable() => Button.onClick.RemoveListener(ButtonClick);
        private void ButtonClick() => OnButtonClick?.Invoke(this);

        public void ChangeState(bool validate = false)
        {
            if (!validate)
                currentState = !currentState;
            
            text.text = title;
            text.fontSize = textSize;
            
            Color color = GetColor();
            text.color = color;
            image.color = color;
        }

        private Color GetColor()
        {
            return currentState ? new Color(0, 0, 0, 1) : new Color(0, 0, 0, 0.6f);
        }
    }
}