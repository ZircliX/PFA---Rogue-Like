using UnityEngine;
using UnityEngine.UI;

namespace DeadLink.Entities
{
    public class EnemyUI : MonoBehaviour
    {
        [SerializeField] private Image healthBar;
        
        public void UpdateHealthBar(float current, float max)
        {
            healthBar.fillAmount = current / max;
        }
    }
}