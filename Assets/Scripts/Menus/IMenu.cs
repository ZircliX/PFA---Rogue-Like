namespace DeadLink.Menus.New
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