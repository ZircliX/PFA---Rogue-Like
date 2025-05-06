using RogueLike.Player;
using UnityEngine;

namespace DeadLink.PowerUpSystem.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/JumpPowerUp", fileName = "JumpPowerUp")]
    public class JumpPowerUp : PowerUp
    {
        [field: SerializeField] public int BonusJumpCount { get; private set; } = 1;

        public override void OnBeUnlocked(RogueLike.Entities.Player player, PlayerMovement playerMovement)
        {
            IsUnlocked = true;
            OnBeUsed(player, playerMovement);
        }

        public override void OnBeUsed(RogueLike.Entities.Player player, PlayerMovement playerMovement)
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

        public override void OnFinishedToBeUsed(RogueLike.Entities.Player player, PlayerMovement playerMovement)
        {
            throw new System.NotImplementedException();
        }
    }
}