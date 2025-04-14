using DeadLink.Menus.Interfaces;
using UnityEngine;

namespace DeadLink.Menus
{
    public abstract class MenuHandler<T> : MonoBehaviour 
        where T : IMenuContext
    {
        public abstract T GetContext();
        public abstract Menu<T> GetMenu();
    }
}