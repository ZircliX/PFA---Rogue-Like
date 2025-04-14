using System;
using System.Collections.Generic;
using DeadLink.Menus.Interfaces;
using LTX.Singletons;
using RogueLike.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DeadLink.Menus
{
    public class MenuManager : MonoSingleton<MenuManager>
    {
        public event Action<MenuType> OnWantsToChangeMenu;
        public event Action<IMenuRunner> OnMenuOpen;
        public event Action<IMenuRunner> OnMenuClose;

        private Stack<IMenuRunner> menuRunners;
        private IMenuRunner currentMenuRunner;

        protected override void Awake()
        {
            base.Awake();
            menuRunners = new Stack<IMenuRunner>();
        }

        public void OpenMenu<T>(Menu<T> menu, MenuHandler<T> handler)
            where T : IMenuContext
        {
            MenuRunner<T> menuRunner = new MenuRunner<T>(menu, handler);

            menuRunners.Push(menuRunner);
            menuRunner.Open();

            currentMenuRunner = menuRunner;

            GameController.CursorVisibility.Write(handler.MenuType, handler.GetContext().CursorVisibility);
            GameController.CursorLockMode.Write(handler.MenuType, handler.GetContext().CursorLockMode);
            GameController.TimeScale.Write(handler.MenuType, handler.GetContext().TimeScale);

            OnMenuOpen?.Invoke(menuRunner);
        }

        public void ChangeMenu(MenuType menu)
        {
            OnWantsToChangeMenu?.Invoke(menu);
        }

        public void CloseMenu(MenuType type)
        {
            if (menuRunners.Count > 0)
            {
                if (currentMenuRunner.GetContext().CanClose)
                {
                    currentMenuRunner.Close();
                    currentMenuRunner = null;
                    menuRunners.Pop();
                }
                else
                {
                    Debug.Log("Cannot close menu, it is blocked");
                }

                if (menuRunners.Count == 0)
                {
                    GameController.CursorVisibility.Write(type, false);
                    GameController.CursorLockMode.Write(type, CursorLockMode.Locked);
                    GameController.TimeScale.Write(type, 1f);
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
                if (currentMenuRunner != null)
                {
                    if (currentMenuRunner.GetContext().CanClose)
                    {
                        CloseMenu(currentMenuRunner.GetContext().MenuType);
                    }
                }
                else
                {
                    //Debug.Log("no menu to close, opening pause menu");
                    ChangeMenu(MenuType.Pause);
                }
            }
        }

        #endregion
    }
}