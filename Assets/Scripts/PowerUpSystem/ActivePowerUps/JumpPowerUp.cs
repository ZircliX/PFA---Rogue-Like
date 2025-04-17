using DeadLink.PowerUpSystem.InterfacePowerUps;
using RogueLike.Player;
using UnityEngine;

namespace DeadLink.PowerUpSystem.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/JumpPowerUp", fileName = "JumpPowerUp")]
    public class JumpPowerUp : PowerUp
    {
        [field: SerializeField] public int BonusJumpCount { get; private set; } = 1;
        public override string Name { get; set; } = "JumpPowerUp";

        
        public override void OnBeUnlocked(IVisitable visitable)
        {
            IsUnlocked = true;
            OnBeUsed(visitable);
        }

        public override void OnBeUsed(IVisitable visitable)
        {
            PlayerMovement playerMovement = visitable as PlayerMovement;
            if (playerMovement != null)
            {
                if (IsUnlocked)
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