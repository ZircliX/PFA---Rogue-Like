using DG.Tweening;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.UI;

namespace DeadLink.Menus.Other
{
    public class WeaponReference : MonoBehaviour
    {
        [SerializeField] private Image activateImage;
        [SerializeField] private Image deactivateImage;
        [SerializeField] private float maxHeight = 80;
        [SerializeField, Self] private LayoutElement layoutElement;
        
        private void OnValidate() => this.ValidateRefs();

        public void ActivateImage()
        {
            activateImage.gameObject.SetActive(true);
            deactivateImage.gameObject.SetActive(false);

            activateImage.DOKill();
            activateImage.DOFade(1, 0.15f);
            deactivateImage.DOKill();
            deactivateImage.DOFade(0, 0.15f);
            
            layoutElement.DOKill();
            layoutElement.DOPreferredSize(new Vector2(0, maxHeight), 0.15f);
        }
        
        public void DeactivateImage()
        {
            activateImage.gameObject.SetActive(false);
            deactivateImage.gameObject.SetActive(true);

            activateImage.DOKill();
            activateImage.DOFade(0, 0.5f);
            deactivateImage.DOKill();
            deactivateImage.DOFade(1, 0.5f);
            
            layoutElement.DOKill();
            layoutElement.DOPreferredSize(new Vector2(0, 0), 0.15f);
        }
    }
}