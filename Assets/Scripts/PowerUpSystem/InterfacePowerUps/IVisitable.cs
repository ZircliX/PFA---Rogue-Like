namespace DeadLink.PowerUpSystem.InterfacePowerUps
{
    public interface IVisitable
    {
        public void Unlock(IVisitor visitor);
        public void Use(string powerUpName);
    }
}