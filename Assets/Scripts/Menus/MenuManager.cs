using System;
using System.Collections.Generic;
using DeadLink.Menus.New.Implementation;
using LTX.ChanneledProperties;
using LTX.Singletons;
using RogueLike.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;
using ZLinq;

namespace DeadLink.Menus.New
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

        private void Start()
        {
            for (int i = 0; i < menus.Length; i++)
            {
                menus[i].Initialize();
            }
        }
        
        public IMenu GetCurrentMenu()
        {
            return openedMenus.Peek();
        }

        public IMenu GetMenu(MenuType menuType)
        {
            return menus.AsValueEnumerable().First(ctx => ctx.MenuType == menuType);
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
            openedMenus.Push(menuToOpen);
            
            MenuProperties menuProperties = menuToOpen.GetMenuProperties();
            UpdateGameProperties(menuProperties);

            OnMenuOpen?.Invoke(menuToOpen);
        }

        public void CloseMenu()
        {
            if (openedMenus.Count > 0)
            {
                if (GetCurrentMenu().GetMenuProperties().CanClose)
                {
                    OnMenuClose?.Invoke(openedMenus.Pop());
                }
            }
        }
        
        public void Pause(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (openedMenus.Count > 0)
                {
                    if (GetCurrentMenu().GetMenuProperties().CanClose)
                    {
                        CloseMenu();
                    }
                }
                else
                {
                    OpenMenu(GetMenu(MenuType.Pause));
                }
            }
        }
    }
}