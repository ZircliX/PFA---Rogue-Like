using LTX.ChanneledProperties;
using UnityEngine;

namespace DeadLink.Menus.Implementation
{
    public class SettingsMenu : Menu
    {
        public override MenuType MenuType { get; protected set; }

        public override MenuProperties GetMenuProperties()
        {
            return new MenuProperties(
                gameObject,
                PriorityTags.Default,
                0f,
                CursorLockMode.None,
                true,
                true,
                false);
        }
        
        private void Awake()
        {
            MenuType = MenuType.Settings;
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