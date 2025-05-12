using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DeadLink.Menus.Other
{
    public class AmmunitionReferences : MonoBehaviour
    {
        [field: SerializeField] public TMP_Text AmmoText { get; private set; }
        [field: SerializeField] public Image AmmoIcon { get; private set; }
        [field: SerializeField] public Image AmmoFill { get; private set; }
        
        public void SetAmmos(int current, int max)
        {
            AmmoText.text = $"{current}/{max}";
            
            DOTween.To(
                () => AmmoFill.fillAmount,
                x => AmmoFill.fillAmount = x,
                (float)current / max,
                0.25f).SetEase(Ease.Linear);
        }
    }
}