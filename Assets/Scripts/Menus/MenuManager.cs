using System;
using System.Collections.Generic;
using DeadLink.Menus.Interfaces;
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
        public event Action<IMenuRunner> OnMenuOpen;
        public event Action<IMenuRunner> OnMenuClose;
        
        private Stack<IMenuRunner> menuRunners;
        private IMenuRunner currentMenuRunner;
        
        public Dictionary<string, MenuHandler<IMenuContext>> Menus { get; private set; }
        [SerializeField, Scene] private MenuHandler<IMenuContext>[] menus; 
        
        protected override void Awake()
        {
            base.Awake();
            
            Menus = new Dictionary<string, MenuHandler<IMenuContext>>();
            for (int i = 0; i < menus.Length; i++)
            {
                MenuHandler<IMenuContext> menuHandler = menus[i];
                Menus.Add(menuHandler.name, menuHandler);
                
                GameController.CursorVisibility.AddPriority(menuHandler, PriorityTags.Highest, false);
                GameController.CursorLockMode.AddPriority(menuHandler, PriorityTags.Highest, CursorLockMode.Locked);
                GameController.TimeScale.AddPriority(menuHandler, menuHandler.GetContext().Priority, 1f);
            }
        }

        public void OpenMenu<T>(Menu<T> menu, MenuHandler<T> handler)
            where T : IMenuContext
        {
            GameController.CursorVisibility.Write(handler, handler.GetContext().CursorVisibility);
            GameController.CursorLockMode.Write(handler, handler.GetContext().CursorLockMode);
            GameController.TimeScale.Write(handler, handler.GetContext().TimeScale);

            MenuRunner<T> menuRunner = new MenuRunner<T>(menu, handler);
            
            menuRunners.Push(menuRunner);
            menuRunner.Open();
            
            currentMenuRunner = menuRunner;
            OnMenuOpen?.Invoke(menuRunner);
        }

        public void ChangeMenu(MenuHandler<IMenuContext> handler)
        {
            OpenMenu(handler.GetMenu(), handler);
        }
        
        public void CloseMenu()
        {
            if (menuRunners.Count > 0)
            {
                IMenuRunner menu = menuRunners.Pop();

                if (!currentMenuRunner.GetContext().CanClose)
                {
                    currentMenuRunner.Close();
                }
                else
                {
                    Debug.Log("Cannot close menu, it is blocked");
                }
                
                if (menuRunners.Count == 0)
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
                var menuHandler = Menus["Pause"];
                var menu = menuHandler.GetMenu();
                
                if (currentMenuRunner != null)
                {
                    if (currentMenuRunner.GetContext().CanStack)
                    {
                        OpenMenu(menu, menuHandler);
                    }
                    else
                    {
                        CloseMenu();
                    }
                }
                else
                {
                    OpenMenu(menu, menuHandler);
                }
            }
        }

        #endregion
    }
}