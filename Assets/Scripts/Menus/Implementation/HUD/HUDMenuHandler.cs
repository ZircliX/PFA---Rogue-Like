
using UnityEngine;

namespace DeadLink.Menus.Implementation
{
    public class HUDMenuHandler : MenuHandler<HUDMenuContext>
    {
        public override HUDMenuContext GetContext()
        {
            return new HUDMenuContext
            {
                GameObject = gameObject,
                CursorLockMode = CursorLockMode.Locked,
                CursorVisibility = false
            };
        }

        public override Menu<HUDMenuContext> GetMenu()
        {
            return new HUDMenu();
        }
    }
}