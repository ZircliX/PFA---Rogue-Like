using UnityEngine;
using UnityEngine.UI;

namespace DeadLink.Menus.Other
{
    public class UpgradeBadge : MonoBehaviour
    {
        [SerializeField] private Image image;

        public void SetImage(Sprite sprite)
        {
            image.sprite = sprite;
        }
    }
}