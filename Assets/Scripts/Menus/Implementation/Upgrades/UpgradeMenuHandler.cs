using UnityEngine;

namespace DeadLink.Menus.Implementation
{
    public class UpgradeMenuHandler : MenuHandler<UpgradeMenuContext>
    {
        [field: SerializeField] public PowerUp.PowerUp[] PowerUps { get; private set; }
        
        public override UpgradeMenuContext GetContext()
        {
            return new UpgradeMenuContext()
            {
                GameObject = gameObject,
                CursorLockMode = CursorLockMode.None,
                CursorVisibility = true
            };
        }

        public override Menu<UpgradeMenuContext> GetMenu()
        {
            return new UpgradeMenu();
        }
        
        public void UseUpgrade(int index)
        {
            //PowerUps[index].Visit();
        }
    }
}