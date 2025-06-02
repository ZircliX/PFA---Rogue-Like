using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DeadLink.Menus.Other
{
    public class UpgradeBadge : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image cooldown;

        public void SetImage(Sprite sprite)
        {
            icon.sprite = sprite;
        }

        public void Use(float current, float max)
        {
            Cooldown(current, max);
        }

        private void Cooldown(float current, float max)
        {
            cooldown.fillAmount = current / max;
        }
    }
}