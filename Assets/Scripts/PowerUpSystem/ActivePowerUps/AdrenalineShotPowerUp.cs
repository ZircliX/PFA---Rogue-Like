using System.Collections;
using RogueLike.Player;
using UnityEngine;

namespace DeadLink.PowerUpSystem.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/AdrenalineShotPowerUp", fileName = "AdrenalineShotPowerUp")]
    public class AdrenalineShotPowerUp : CooldownPowerUp
    {
        [field: SerializeField] public int AdrenalineMultiplier { get; private set; } = 2;

        
        public override void OnBeUnlocked(RogueLike.Entities.Player player, PlayerMovement playerMovement)
        {
            IsUnlocked = true;
            CanBeUsed = true;
        }

        public override void OnBeUsed(RogueLike.Entities.Player player, PlayerMovement playerMovement)
        {
            if (IsUnlocked && CanBeUsed)
            {
                player.OnAdrenalineShot(AdrenalineMultiplier);
                playerMovement.StartCooldownCoroutine(this);
                player.StartCoroutine(CompetenceDuration(player, playerMovement, OnFinishedToBeUsed));
            }
        }

        public override void OnFinishedToBeUsed(RogueLike.Entities.Player player, PlayerMovement playerMovement)
        {
            player.OnAdrenalineShotEnd();
        }
    }
}