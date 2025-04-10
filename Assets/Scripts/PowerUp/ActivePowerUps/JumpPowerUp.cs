using DeadLink.PowerUp.Components;
using UnityEngine;

namespace DeadLink.PowerUp.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/JumpPowerUp", fileName = "JumpPowerUp")]
    public class JumpPowerUp : PowerUp
    {
        [field: SerializeField] public int BonusJumpCount { get; private set; } = 1;
        
        public override void Visit(VisitableComponent visitable)
        {
            JumpComponent jumpComponent = visitable as JumpComponent;
            if (jumpComponent != null)
            {
                jumpComponent.RemainingJump += BonusJumpCount;
                Debug.Log("Visitor accepted in JumpComponent");
            }
        }
    }
}