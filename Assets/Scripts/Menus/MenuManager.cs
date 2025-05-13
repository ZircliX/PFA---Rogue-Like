using System;
using System.Collections.Generic;
using DeadLink.Menus.Implementation;
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
            return menus.AsValueEnumerable().First(ctx => ctx.MenuType == menuType);
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