using UnityEngine;

namespace DeadLink.Menus.Implementation
{
    public class UpgradeMenu : Menu
    {
        public override bool CanClose { get; protected set; } = false;
        
        [field: SerializeField] public PowerUp.PowerUp[] PowerUps { get; private set; }

        public void UseUpgrade(int index)
        {
            //PowerUps[index].Visit();
        }
    }
}