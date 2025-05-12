namespace DeadLink.Menus
{
    public interface IMenu
    {
        bool BaseState { get; set; }
        public abstract MenuType MenuType { get; }
        
        void Initialize();

        void Open();
        void Close();
        
        MenuProperties GetMenuProperties();
    }
}