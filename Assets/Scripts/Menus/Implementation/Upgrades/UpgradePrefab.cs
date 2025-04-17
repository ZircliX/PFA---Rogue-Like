using DeadLink.PowerUpSystem;
using DeadLink.PowerUpSystem.InterfacePowerUps;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DeadLink.Menus.Implementation
{
    public class UpgradePrefab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text description;
        [SerializeField] private Image icon;
        
        public PowerUp powerUp { get; private set; }
        [field : SerializeField] public IVisitable visitable { get; private set; }
        
        public void Initialize(string title, string description, Sprite icon, PowerUp powerUp)
        {
            this.title.text = title;
            this.description.text = description;
            this.icon.sprite = icon;
            
            this.powerUp = powerUp;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Enter");
            transform.DOScale(1.1f, 0.25f).SetUpdate(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(1f, 0.25f).SetUpdate(true);
        }
    }
}