using LTX.ChanneledProperties;
using RogueLike;
using UnityEngine;

namespace DeadLink.Menus.Implementation
{
    public class SettingsMenuHandler : MenuHandler<SettingsMenuContext>
    {
        [field: SerializeField] protected override bool baseState { get; set; }

        public override MenuType MenuType => MenuType.Settings;

        protected override void CheckMenuType(MenuType type)
        {
            if (type == GameMetrics.Global.SettingsMenu)
            {
                SettingsMenu menu = new SettingsMenu();
                MenuManager.Instance.OpenMenu(menu, this);
            }
        }

        public override SettingsMenuContext GetContext()
        {
            return new SettingsMenuContext
            {
                GameObject = gameObject,
                TimeScale = 0f,
                CursorLockMode = CursorLockMode.None,
                CursorVisibility = true,
                CanClose = true,
                CanStack = false,
                Priority = PriorityTags.Default,
                MenuType = MenuType,
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

        public void EnableVibrations(bool state)
        {
            
        }

        public void ChangeLanguage(int index)
        {
            
        }
        
        public void ChangeMasterVolume(float value)
        {
            //AudioManager.Instance.SetMasterVolume(value);
        }
        
        public void ChangeMusicVolume(float value)
        {
            //AudioManager.Instance.SetMasterVolume(value);
        }
        
        public void ChangeSFXVolume(float value)
        {
            //AudioManager.Instance.SetMasterVolume(value);
        }
    }
}