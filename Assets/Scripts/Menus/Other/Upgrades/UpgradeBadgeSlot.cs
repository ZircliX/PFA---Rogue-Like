using UnityEngine;

namespace DeadLink.Menus.Other
{
    public class UpgradeBadgeSlot : MonoBehaviour
    {
        private UpgradeBadge upgradeBadge;
        
        public bool IsEmpty => upgradeBadge == null;
        
        public void SetUpgradeBadge(UpgradeBadge badge)
        {
            if (upgradeBadge != null)
            {
                Destroy(upgradeBadge.gameObject);
            }
            
            upgradeBadge = Instantiate(badge, transform);
            upgradeBadge.transform.localPosition = Vector3.zero;
            upgradeBadge.transform.localScale = Vector3.one;
        }
    }
}