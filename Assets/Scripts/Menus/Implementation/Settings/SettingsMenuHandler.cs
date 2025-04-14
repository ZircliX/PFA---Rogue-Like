using UnityEngine;

namespace DeadLink.Menus.Implementation
{
    public class SettingsMenuHandler : MenuHandler<SettingsMenuContext>
    {
        public override SettingsMenuContext GetContext()
        {
            return new SettingsMenuContext
            {
                GameObject = gameObject,
                CursorLockMode = CursorLockMode.None,
                CursorVisibility = true
            };
        }

        public override Menu<SettingsMenuContext> GetMenu()
        {
            return new SettingsMenu();
        }
        
        public void Back()
        {
            MenuManager.Instance.CloseMenu();
        }
    }
}