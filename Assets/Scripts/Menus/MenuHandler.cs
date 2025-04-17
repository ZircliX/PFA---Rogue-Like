using DeadLink.Menus.Interfaces;
using UnityEngine;

namespace DeadLink.Menus
{
    public abstract class MenuHandler<T> : MonoBehaviour
        where T : IMenuContext
    {
        protected abstract bool baseState { get; set; }
        public abstract MenuType MenuType { get; }

        protected virtual void Awake()
        {
            MenuManager.Instance.OnWantsToChangeMenu += CheckMenuType;
            gameObject.SetActive(baseState);
        }

        protected abstract void CheckMenuType(MenuType type);

        public abstract T GetContext();
        public abstract Menu<T> GetMenu();
    }
}