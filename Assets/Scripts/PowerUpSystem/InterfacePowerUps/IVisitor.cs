namespace DeadLink.PowerUpSystem.InterfacePowerUps
{
    public interface IVisitor
    {
        public abstract string Name { get; set; }
        public void OnBeUnlocked(IVisitable visitable);
        public void OnBeUsed(IVisitable visitable);
        
    }
}