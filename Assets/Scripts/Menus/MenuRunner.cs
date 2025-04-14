using DeadLink.Menus.Interfaces;

namespace DeadLink.Menus
{
    public sealed class MenuRunner<T> : IMenuRunner
        where T : IMenuContext
    {
        public IMenu Menu => menu;

        private readonly Menu<T> menu;
        private readonly MenuHandler<T> menuHandler;
        
        public MenuRunner(Menu<T> menu, MenuHandler<T> menuHandler)
        {
            this.menu = menu;
            this.menuHandler = menuHandler;
        }
        
        public void Open()
        {
            T context = menuHandler.GetContext();
            menu.OnOpen(ref context);
        }

        public void Refresh()
        {
            T context = menuHandler.GetContext();
            menu.OnRefresh(ref context);
        }

        public void Close()
        {
            T context = menuHandler.GetContext();
            menu.OnClose(ref context);
        }

        public IMenuContext GetContext()
        {
            return menuHandler.GetContext();
        }
    }
}