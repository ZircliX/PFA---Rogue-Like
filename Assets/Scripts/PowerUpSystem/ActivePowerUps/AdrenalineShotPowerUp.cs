using System.Collections;
using DeadLink.Player;
using RogueLike.Controllers;
using RogueLike.Player;
using UnityEngine;

namespace DeadLink.PowerUpSystem.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/AdrenalineShotPowerUp", fileName = "AdrenalineShotPowerUp")]
    public class AdrenalineShotPowerUp : CooldownPowerUp
    {
        [field: SerializeField] public int AdrenalineMultiplier { get; private set; } = 2;

        
        public override void OnBeUnlocked(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            base.OnBeUnlocked(playerEntity, playerMovement);
            IsUnlocked = true;
            CanBeUsed = true;
        }

        public override void OnBeUsed(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            if (IsUnlocked && CanBeUsed)
            {
                CameraVfxTransformHandler.Instance.OverdriveComponent.PlayVFXCamera(20f, CameraVfxTransformHandler.Instance.OverdrivePosition);

                playerEntity.OnAdrenalineShot(AdrenalineMultiplier);
                playerMovement.StartCooldownCoroutine(this);
                playerEntity.StartCoroutine(CompetenceDuration(playerEntity, playerMovement, OnFinishedToBeUsed));
            }
        }

        public override void OnFinishedToBeUsed(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            playerEntity.OnAdrenalineShotEnd();
        }
        
        public override void OnReset(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            OnFinishedToBeUsed(playerEntity, playerMovement);
            IsUnlocked = false;
        }
    }
}