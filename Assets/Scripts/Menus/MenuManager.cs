using System.Collections.Generic;
using KBCore.Refs;
using LTX.ChanneledProperties;
using LTX.Singletons;
using RogueLike.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DeadLink.Menus
{
    public class MenuManager : MonoSingleton<MenuManager>
    {
        private class MenuData
        {
            public readonly bool CanStack;
            public readonly bool Blocked;
            public readonly Menu Menu;
            
            public MenuData(Menu menu, bool canStack = true, bool blocked = false)
            {
                Menu = menu;
                Blocked = blocked;
                CanStack = canStack;
            }
        }

        [SerializeField, Scene] private Menu[] menus; 
        private Stack<MenuData> openedMenus;
        private MenuData currentMenu;
        public Dictionary<string, Menu> Menus { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();
            GameController.CursorVisibility.AddPriority(this, PriorityTags.Highest, false);
            GameController.CursorLockMode.AddPriority(this, PriorityTags.Highest, CursorLockMode.Locked);

            openedMenus = new Stack<MenuData>(8);
            
            Menus = new Dictionary<string, Menu>();
            for (int i = 0; i < menus.Length; i++)
            {
                Menu menu = menus[i];
                Menus.Add(menu.MenuName, menu);
            }
        }

        public void OpenMenu(Menu menu, bool blocked = false)
        {
            GameController.CursorVisibility.Write(this, true);
            GameController.CursorLockMode.Write(this, CursorLockMode.None);

            MenuData menuData = new MenuData(menu, blocked);
            currentMenu = menuData;
            
            openedMenus.Push(currentMenu);
            menu.OnOpen();

        }
        
        public void CloseMenu()
        {
            if (openedMenus.Count > 0)
            {
                MenuData menu = openedMenus.Pop();

                if (!menu.Blocked && menu.Menu.CanClose)
                {
                    menu.Menu.OnClose();
                }
                else
                {
                    Debug.Log("Cannot close menu, it is blocked");
                }
                
                if (openedMenus.Count == 0)
                {
                    GameController.CursorVisibility.Write(this, false);
                    GameController.CursorLockMode.Write(this, CursorLockMode.Locked);
                }
            }
            else
            {
                Debug.Log("No menu to close");
            }
        }

        #region Inputs

        public void Pause(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (currentMenu != null)
                {
                    if (currentMenu.CanStack)
                    {
                        OpenMenu(Menus["Pause"]);
                    }
                    else
                    {
                        CloseMenu();
                    }
                }
                else
                {
                    OpenMenu(Menus["Pause"]);
                }
            }
        }

        #endregion
    }
}