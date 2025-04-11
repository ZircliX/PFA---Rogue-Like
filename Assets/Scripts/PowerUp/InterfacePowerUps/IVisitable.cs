namespace DeadLink.PowerUp
{
    public interface IVisitable
    {
        public void Unlock(IVisitor visitor);
        public void Use(IVisitor visitor);
    }
}