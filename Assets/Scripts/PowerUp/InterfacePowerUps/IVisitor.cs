using DeadLink.PowerUp.Components;
using RogueLike.Player;

namespace DeadLink.PowerUp
{
    public interface IVisitor
    {
        public void OnBeUnlocked(VisitableComponent visitable);
        public void OnBeUsed(VisitableComponent visitable);
        
    }
}