using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DeadLink.Entities
{
    public class EnemyUI : MonoBehaviour
    {
        [SerializeField] private Image healthBar;
        [SerializeField] private Image healthBarBg;
        
        public void UpdateHealthBar(float current, float max)
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(healthBar.DOFillAmount(current / max, 0.20f))
                .SetDelay(0.1f)
                .Append(healthBarBg.DOFillAmount(current / max, 0.25f));

            sequence.SetTarget(gameObject);
            
            sequence.Play();
        }
    }
}