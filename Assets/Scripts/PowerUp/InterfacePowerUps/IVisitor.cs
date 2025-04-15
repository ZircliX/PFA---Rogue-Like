using DeadLink.PowerUp.Components;
using RogueLike.Player;

namespace DeadLink.PowerUp
{
    public interface IVisitor
    {
        public abstract string Name { get; set; }
        public void OnBeUnlocked(VisitableComponent visitable);
        public void OnBeUsed(VisitableComponent visitable);
        
    }
}