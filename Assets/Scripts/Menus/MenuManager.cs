using System;
using System.Collections.Generic;
using DeadLink.Menus.Interfaces;
using LTX.ChanneledProperties;
using LTX.Singletons;
using RogueLike.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DeadLink.Menus
{
    public class MenuManager : MonoSingleton<MenuManager>
    {
        private struct MenuInfoCarryMoi
        {
            public IMenuRunner Runner;
            public IMenuContext Context;
        }
        
        public event Action<MenuType> OnWantsToChangeMenu;
        public event Action<IMenuRunner> OnMenuOpen;
        public event Action<IMenuRunner> OnMenuClose;

        private Stack<MenuInfoCarryMoi> menuRunners;
        private IMenuRunner currentMenuRunner;

        protected override void Awake()
        {
            base.Awake();

            GameController.CursorVisibility.AddPriority(this, PriorityTags.None);
            GameController.CursorLockMode.AddPriority(this, PriorityTags.None);
            GameController.TimeScale.AddPriority(this, PriorityTags.None);
            
            menuRunners = new Stack<MenuInfoCarryMoi>();
        }

        public void OpenMenu<T>(Menu<T> menu, MenuHandler<T> handler)
            where T : IMenuContext
        {
            MenuRunner<T> menuRunner = new MenuRunner<T>(menu, handler);

            menuRunners.Push(new MenuInfoCarryMoi()
            {
                Runner = menuRunner,
                Context = handler.GetContext()
            });
            menuRunner.Open();

            currentMenuRunner = menuRunner;

            T menuContext = handler.GetContext();
            
            GameController.CursorVisibility.ChangeChannelPriority(this, menuContext.Priority);
            GameController.CursorLockMode.ChangeChannelPriority(this, menuContext.Priority);
            GameController.TimeScale.ChangeChannelPriority(this, menuContext.Priority);
            
            GameController.CursorVisibility.Write(this, menuContext.CursorVisibility);
            GameController.CursorLockMode.Write(this, menuContext.CursorLockMode);
            GameController.TimeScale.Write(this, menuContext.TimeScale);

            OnMenuOpen?.Invoke(menuRunner);
        }

        public void ChangeMenu(MenuType menu)
        {
            OnWantsToChangeMenu?.Invoke(menu);
        }

        public void CloseMenu()
        {
            if (menuRunners.Count > 0)
            {
                if (currentMenuRunner.GetContext().CanClose)
                {
                    currentMenuRunner.Close();
                    menuRunners.Pop();

                    if (menuRunners.TryPeek(out MenuInfoCarryMoi info))
                    {
                        currentMenuRunner = info.Runner;
                    }
                    else
                    {
                        currentMenuRunner = null;
                    }
                }
                else
                {
                    Debug.Log("Cannot close menu, it is blocked");
                }
            }
            
            if (menuRunners.Count == 0)
            {
                GameController.CursorVisibility.ChangeChannelPriority(this, PriorityTags.None);
                GameController.CursorLockMode.ChangeChannelPriority(this, PriorityTags.None);
                GameController.TimeScale.ChangeChannelPriority(this, PriorityTags.None);
            }
            else if (menuRunners.TryPeek(out MenuInfoCarryMoi info))
            {
                IMenuContext menuContext = info.Context;

                GameController.CursorVisibility.ChangeChannelPriority(this, menuContext.Priority);
                GameController.CursorLockMode.ChangeChannelPriority(this, menuContext.Priority);
                GameController.TimeScale.ChangeChannelPriority(this, menuContext.Priority);
            
                GameController.CursorVisibility.Write(this, menuContext.CursorVisibility);
                GameController.CursorLockMode.Write(this, menuContext.CursorLockMode);
                GameController.TimeScale.Write(this, menuContext.TimeScale);
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
                        CloseMenu();
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