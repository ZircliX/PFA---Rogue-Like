using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DeadLink.Menus.Implementation
{
    public class UpgradePrefab : MonoBehaviour
    {
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text description;
        [SerializeField] private Image icon;
        
        public void Initialize(string title, string description, Sprite icon)
        {
            this.title.text = title;
            this.description.text = description;
            this.icon.sprite = icon;
        }
    }
}