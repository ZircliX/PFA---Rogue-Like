namespace DeadLink.PowerUp
{
    public interface IVisitable
    {
        public void Accept(IVisitor visitor);
    }
}