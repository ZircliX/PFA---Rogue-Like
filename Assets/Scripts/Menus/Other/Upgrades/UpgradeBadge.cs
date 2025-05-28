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

        public void Use(float cd)
        {
            StartCoroutine(Cooldown(cd));
        }

        private IEnumerator Cooldown(float cd)
        {
            float currentCooldown = cd;

            while (currentCooldown > 0)
            {
                yield return null;
                currentCooldown -= Time.deltaTime;
                cooldown.fillAmount = currentCooldown - cd;
            }
        }
    }
}