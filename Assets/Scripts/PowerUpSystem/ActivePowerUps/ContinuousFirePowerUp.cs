using DeadLink.Player;
using RogueLike.Controllers;
using RogueLike.Player;
using UnityEngine;

namespace DeadLink.PowerUpSystem.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/ContinuousFirePowerUp", fileName = "ContinuousFirePowerUp")]
    public class ContinuousFirePowerUp : CooldownPowerUp
    {
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
                //CameraVfxTransformHandler.Instance.PermaShotComponent.PlayVFXCamera(2.5f, CameraVfxTransformHandler.Instance.PermaShotPosition);

                CanBeUsed = true;
                playerEntity.ActiveContinuousFire(this);
                playerEntity.StartCoroutine(Cooldown());
                playerEntity.StartCoroutine(CompetenceDuration(playerEntity, playerMovement, OnFinishedToBeUsed));
            }
        }

        public override void OnFinishedToBeUsed(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            playerEntity.DesactiveContinuousFire();
        }

        public override void OnReset(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            OnFinishedToBeUsed(playerEntity, playerMovement);
            IsUnlocked = false;
        }
    }
}