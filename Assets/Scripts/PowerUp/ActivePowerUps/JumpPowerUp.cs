using DeadLink.PowerUp.Components;
using RogueLike.Player;
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
            OnBeUsed(visitable);
        }

        public override void OnBeUsed(VisitableComponent visitable)
        {
            PlayerMovement playerMovement = visitable as PlayerMovement;
            if (playerMovement != null)
            {
                if (isUnlocked)
                {
                    playerMovement.AddBonusJump(BonusJumpCount);
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