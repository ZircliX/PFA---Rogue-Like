namespace DeadLink.Menus.Interfaces
{
    public interface IMenuRunner
    {
        IMenu Menu { get; }
        
        void Open();
        void Refresh();
        void Close();

        IMenuContext GetContext();
    }
}