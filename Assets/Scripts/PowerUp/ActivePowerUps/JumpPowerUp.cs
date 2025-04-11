using DeadLink.PowerUp.Components;
using UnityEngine;

namespace DeadLink.PowerUp.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/JumpPowerUp", fileName = "JumpPowerUp")]
    public class JumpPowerUp : PowerUp
    {
        [field: SerializeField] public int BonusJumpCount { get; private set; } = 1;
        
        public override void OnBeUnlocked(VisitableComponent visitable)
        {
            isUnlocked = true;
        }

        public override void OnBeUsed(VisitableComponent visitable)
        {
            JumpComponent jumpComponent = visitable as JumpComponent;
            if (jumpComponent != null)
            {
                if (isUnlocked)
                {
                    jumpComponent.RemainingJump += BonusJumpCount;
                    Debug.Log("Visitor accepted in JumpComponent");
                }   
                else
                {
                    Debug.Log("PowerUp is not unlocked");
                }
            }
        }
    }
}