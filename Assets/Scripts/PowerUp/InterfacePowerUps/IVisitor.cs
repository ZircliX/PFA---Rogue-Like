using DeadLink.PowerUp.Components;
using RogueLike.Player;

namespace DeadLink.PowerUp
{
    public interface IVisitor
    {
        public void Visit(VisitableComponent visitable);
        
    }
}