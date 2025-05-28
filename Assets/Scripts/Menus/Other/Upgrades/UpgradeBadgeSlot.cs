using DeadLink.PowerUpSystem;
using UnityEngine;

namespace DeadLink.Menus.Other
{
    public class UpgradeBadgeSlot : MonoBehaviour
    {
        private UpgradeBadge upgradeBadge;
        
        public bool IsEmpty => upgradeBadge == null;
        
        public void SetUpgradeBadge(UpgradeBadge badgePrefab, PowerUp pu)
        {
            if (upgradeBadge != null)
            {
                Destroy(upgradeBadge.gameObject);
            }
            
            upgradeBadge = Instantiate(badgePrefab, transform);
            //upgradeBadge.transform.localPosition = Vector3.zero;
            //upgradeBadge.transform.localScale = Vector3.one;
            
            upgradeBadge.SetImage(pu.Badge);
        }

        public void Delete()
        {
            if (upgradeBadge != null)
            {
                Destroy(upgradeBadge.gameObject);
                upgradeBadge = null;
            }
        }
    }
}