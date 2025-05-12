using UnityEngine;
using UnityEngine.UI;

namespace DeadLink.Menus.Other
{
    public class WeaponReference : MonoBehaviour
    {
        [SerializeField] private Image activateImage;
        [SerializeField] private Image deactivateImage;

        public void ActivateImage()
        {
            activateImage.gameObject.SetActive(true);
            deactivateImage.gameObject.SetActive(false);
        }
        
        public void DeactivateImage()
        {
            activateImage.gameObject.SetActive(false);
            deactivateImage.gameObject.SetActive(true);
        }
    }
}