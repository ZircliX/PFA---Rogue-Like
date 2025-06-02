using System;
using KBCore.Refs;
using RogueLike;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DeadLink.Menus
{
    public class ButtonHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Header("References")]
        [field: SerializeField, Self] public Image OriginalImage { get; private set; } 
        [field: SerializeField] public Sprite HighlightedImage { get; private set; } 
        [field: SerializeField] public Material Material { get; private set; } 
        [field: SerializeField] public TextMeshProUGUI Text { get; private set; } 
        [field: SerializeField] public Color OriginalColor { get; private set; } 
        [field: SerializeField] public Color Color { get; private set; }
        public bool IsSelected;
        [field: SerializeField] public bool IsSelectionable { get; private set; } 
        [field: SerializeField, Self] public Button button { get; private set; }
        
        public Sprite _image;

        private void OnValidate()
        {
            this.ValidateRefs();
        }

        private void Awake()
        { 
            _image = OriginalImage.sprite;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!button.IsInteractable() || IsSelected) return;
            AudioManager.Global.PlayOneShot(GameMetrics.Global.FMOD_UIHover, eventData.position);
            if (OriginalImage != null)
            {
                OriginalImage.material = Material;
            }

            if (HighlightedImage != null)
            {
                OriginalImage.sprite = HighlightedImage;
            }

            if (Text != null)
            {
                Text.color = Color;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!button.IsInteractable() || IsSelected) return;
            if (OriginalImage != null)
            {
                OriginalImage.material = null;  
            }
            
            if (HighlightedImage != null)
            {
                OriginalImage.sprite = _image;
            }
            
            if (Text != null)
            {
                Text.color = OriginalColor;
            }
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (IsSelectionable && !IsSelected)
            {
                ButtonSelectioner.Instance.Select(this);
            }
            AudioManager.Global.PlayOneShot(GameMetrics.Global.FMOD_UIClick, eventData.position);
        }
    }
}