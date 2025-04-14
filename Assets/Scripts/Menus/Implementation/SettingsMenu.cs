namespace DeadLink.Menus.Implementation
{
    public class SettingsMenu : Menu
    {
        public override bool CanClose { get; protected set; } = true;
        
        public void Back()
        {
            MenuManager.Instance.CloseMenu();
        }
    }
}