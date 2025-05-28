using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DeadLink.Menus
{
    public class HoverShaderUi : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("References")]
        [field: SerializeField] public Image Image { get; private set; } 
        [field: SerializeField] public Material Material { get; private set; } 
        [field: SerializeField] public TextMeshProUGUI Text { get; private set; } 
        [field: SerializeField] public Color OriginalColor { get; private set; } 
        [field: SerializeField] public Color Color { get; private set; } 

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Image != null)
            {
                Image.material = Material;
            }

            if (Text != null)
            {
                Text.color = Color;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (Image != null)
            {
                Image.material = null;  
            }
            
            if (Text != null)
            {
                Text.color = OriginalColor;
            }
        }
        
        
    }
}