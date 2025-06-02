using DeadLink.PowerUpSystem;
using UnityEngine;

namespace DeadLink.Menus.Other
{
    public class UpgradeBadgeSlot : MonoBehaviour
    {
        public UpgradeBadge UpgradeBadge { get; private set; }
        public PowerUp PowerUp { get; private set; }
        
        public bool IsEmpty => UpgradeBadge == null;
        
        public void SetUpgradeBadge(UpgradeBadge badgePrefab, PowerUp pu)
        {
            if (UpgradeBadge != null)
            {
                Destroy(UpgradeBadge.gameObject);
            }
            
            UpgradeBadge = Instantiate(badgePrefab, transform);
            //upgradeBadge.transform.localPosition = Vector3.zero;
            //upgradeBadge.transform.localScale = Vector3.one;
            
            UpgradeBadge.SetImage(pu.Badge);
            PowerUp = pu;
        }

        public void Delete()
        {
            if (UpgradeBadge != null)
            {
                Destroy(UpgradeBadge.gameObject);
                UpgradeBadge = null;
                PowerUp = null;
            }
        }
    }
}