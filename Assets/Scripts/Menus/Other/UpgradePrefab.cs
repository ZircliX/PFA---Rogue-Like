using DeadLink.PowerUpSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DeadLink.Menus.Implementation
{
    public class UpgradePrefab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text description;
        [SerializeField] private Image icon;
        
        public PowerUp powerUp { get; private set; }
        
        [SerializeField] private  float tiltAmount = 10f;
        [SerializeField] private  float smoothSpeed = 5f;

        private RectTransform rectTransform;
        private static UpgradePrefab currentlyHovered;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }
        
        private void Update()
        {
            if (currentlyHovered == this)
            {
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, null, out Vector2 localMousePosition))
                {
                    // Normalize mouse position relative to the RectTransform center
                    float normalizedX = Mathf.Clamp((localMousePosition.x / rectTransform.rect.width) * 2f, -1f, 1f);
                    float normalizedY = Mathf.Clamp((localMousePosition.y / rectTransform.rect.height) * 2f, -1f, 1f);

                    // Apply tilt (clamped between -tiltAmount and +tiltAmount)
                    float tiltX = Mathf.Clamp(normalizedY * tiltAmount, -tiltAmount, tiltAmount);
                    float tiltY = Mathf.Clamp(-normalizedX * tiltAmount, -tiltAmount, tiltAmount);

                    Quaternion targetRotation = Quaternion.Euler(tiltX, tiltY, 0);
                    rectTransform.rotation = Quaternion.Lerp(rectTransform.rotation, targetRotation, 0.01f * smoothSpeed);
                }
            }
            else
            {
                // Reset rotation smoothly
                rectTransform.rotation = Quaternion.Lerp(rectTransform.rotation, Quaternion.identity, 0.01f * smoothSpeed);
            }
        }
        
        public void Initialize(string title, string description, Sprite icon, PowerUp powerUp)
        {
            this.title.text = title;
            this.description.text = description;
            this.icon.sprite = icon;
            
            this.powerUp = powerUp;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //transform.DOScale(1.1f, 0.25f).SetUpdate(true);
            currentlyHovered = this;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //transform.DOScale(1f, 0.25f).SetUpdate(true);
            if (currentlyHovered == this)
                currentlyHovered = null;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
        }
    }
}