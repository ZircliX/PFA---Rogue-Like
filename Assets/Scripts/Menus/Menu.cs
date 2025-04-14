using DeadLink.Menus.Interfaces;

namespace DeadLink.Menus
{
    public abstract class Menu<T> : IMenu 
        where T : IMenuContext
    {
        public virtual void OnOpen(ref T context)
        {
            context.GameObject.SetActive(true);
        }

        public abstract void OnRefresh(ref T context);

        public virtual void OnClose(ref T context)
        {
            context.GameObject.SetActive(true);
        }
    }
}