using System;
using System.Collections.Generic;
using System.Linq;
using DeadLink.Menus.Implementation;
using DeadLink.SceneManagement;
using LTX.ChanneledProperties;
using LTX.Singletons;
using RogueLike.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;
using ZLinq;

namespace DeadLink.Menus
{
    public class MenuManager : MonoSingleton<MenuManager>
    {
        [SerializeField] private Menu[] menus;
        [field: SerializeField] public HUDMenu HUDMenu { get; private set; }
        
        public event Action<IMenu> OnMenuOpen;
        public event Action<IMenu> OnMenuClose;
        
        private Stack<IMenu> openedMenus;
        
        protected override void Awake()
        {
            base.Awake();

            GameController.CursorVisibility.AddPriority(this, PriorityTags.None);
            GameController.CursorLockMode.AddPriority(this, PriorityTags.None);
            GameController.TimeScale.AddPriority(this, PriorityTags.None);
            
            openedMenus = new Stack<IMenu>();
        }

        protected override void OnDestroy()
        {
            //Pourquoi je l'ai pas fait avant, ZircliX tu pues
            GameController.CursorVisibility.RemovePriority(this);
            GameController.CursorLockMode.RemovePriority(this);
            GameController.TimeScale.RemovePriority(this);
            base.OnDestroy();
        }

        private void OnEnable()
        {
            SceneController.Global.OnWantsToChangeScene += ResetPriorities;
        }
        
        private void OnDisable()
        {
            SceneController.Global.OnWantsToChangeScene -= ResetPriorities;
        }

        private void Start()
        {
            for (int i = 0; i < menus.Length; i++)
            {
                Menu menuToOpen = menus[i];
                
                menuToOpen.Initialize();
                if (menuToOpen.BaseState)
                {
                    OpenMenu(menuToOpen);
                }
            }
        }
        
        public bool TryGetCurrentMenu(out IMenu IMenu)
        {
            IMenu = default;
            if (HasOpenMenu())
            {
                IMenu = openedMenus.Peek();
            }

            return IMenu != default;
        }

        public IMenu GetMenu(MenuType menuType)
        {
            for (int i = 0; i < menus.Length; i++)
            {
                if (menus[i].MenuType == menuType) return menus[i];
            }

            return menus.FirstOrDefault(ctx => ctx.MenuType == menuType);
        }

        private void ResetPriorities()
        {
            GameController.CursorVisibility.ChangeChannelPriority(this, PriorityTags.None);
            GameController.CursorLockMode.ChangeChannelPriority(this, PriorityTags.None);
            GameController.TimeScale.ChangeChannelPriority(this, PriorityTags.None);
        }
        
        private void UpdateGameProperties(MenuProperties menuProperties)
        {
            GameController.CursorVisibility.ChangeChannelPriority(this, menuProperties.Priority);
            GameController.CursorLockMode.ChangeChannelPriority(this, menuProperties.Priority);
            GameController.TimeScale.ChangeChannelPriority(this, menuProperties.Priority);
            
            GameController.CursorVisibility.Write(this, menuProperties.CursorVisibility);
            GameController.CursorLockMode.Write(this, menuProperties.CursorLockMode);
            GameController.TimeScale.Write(this, menuProperties.TimeScale);
        }

        public void OpenMenu(IMenu menuToOpen)
        {
            //Debug.Log($"MenuToOpen : {menuToOpen}, menuType : {menuToOpen.MenuType}, menuName : {menuToOpen.GetMenuProperties().GameObject}");

            if (HasOpenMenu() && !openedMenus.Peek().GetMenuProperties().CanStack) return;
            if (menuToOpen is SettingsMenu sm && openedMenus.Peek().MenuType == MenuType.Pause)
            {
                openedMenus.Peek().GetMenuProperties().GameObject.SetActive(false);
            }
            
            openedMenus.Push(menuToOpen);
            
            MenuProperties menuProperties = menuToOpen.GetMenuProperties();
            UpdateGameProperties(menuProperties);
            
            menuToOpen.Open();

            OnMenuOpen?.Invoke(menuToOpen);
        }

        public void CloseMenu(bool FORCE_CLOSE = false)
        {
            if ((HasOpenMenu() && openedMenus.Peek().GetMenuProperties().CanClose) || FORCE_CLOSE)
            {
                IMenu menuToClose = openedMenus.Pop();
                menuToClose.Close();
                OnMenuClose?.Invoke(menuToClose);

                if (menuToClose is SettingsMenu sm && openedMenus.Peek().MenuType == MenuType.Pause)
                {
                    openedMenus.Peek().GetMenuProperties().GameObject.SetActive(true);
                }
                
            
                IMenu menu = openedMenus.Peek();
                UpdateGameProperties(menu.GetMenuProperties());
            }
        }
        
        public void Pause(InputAction.CallbackContext context)
        {
            if (context.performed && HasOpenMenu())
            {
                if (TryGetCurrentMenu(out IMenu IMenu))
                {
                    MenuProperties menuProperties = IMenu.GetMenuProperties();
                
                    if (menuProperties.CanStack && IMenu.MenuType != MenuType.Pause)
                    {
                        OpenMenu(GetMenu(MenuType.Pause));
                    }
                    else if (menuProperties.CanClose)
                    {
                        CloseMenu();
                    }
                }
            }
        }
        
        private bool HasOpenMenu() => openedMenus.Count > 0;
    }
}