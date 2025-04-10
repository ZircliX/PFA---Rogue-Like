namespace DeadLink.PowerUp.InterfacePowerUps
{
    public interface IVisitable
    {
        public void Accept(IVisitor visitor);
    }
}