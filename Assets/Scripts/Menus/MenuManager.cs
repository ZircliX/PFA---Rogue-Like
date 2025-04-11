using System.Collections.Generic;
using LTX.ChanneledProperties;
using LTX.Singletons;
using RogueLike.Controllers;
using UnityEngine;

namespace DeadLink.Menus
{
    public class MenuManager : MonoSingleton<MenuManager>
    {
        private Stack<Menu> openedMenus;
        
        protected override void Awake()
        {
            base.Awake();
            GameController.CursorVisibility.AddPriority(this, PriorityTags.Highest, false);
            GameController.CursorLockMode.AddPriority(this, PriorityTags.Highest, CursorLockMode.Locked);

            openedMenus = new Stack<Menu>(8);
        }
        
        public void OpenMenu(Menu menu)
        {
            GameController.CursorVisibility.Write(this, true);
            GameController.CursorLockMode.Write(this, CursorLockMode.None);
            
            openedMenus.Push(menu);
            menu.OnOpen();
        }
        
        public void CloseMenu()
        {
            if (openedMenus.Count > 0)
            {
                Menu menu = openedMenus.Pop();
                menu.OnOpen();
                
                if (openedMenus.Count == 0)
                {
                    GameController.CursorVisibility.Write(this, false);
                    GameController.CursorLockMode.Write(this, CursorLockMode.Locked);
                }
            }
        }
    }
}