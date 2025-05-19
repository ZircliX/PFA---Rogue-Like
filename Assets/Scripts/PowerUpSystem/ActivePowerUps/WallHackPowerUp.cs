using DeadLink.Entities;
using RogueLike.Player;
using UnityEngine;

namespace DeadLink.PowerUpSystem.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/WallHackPowerUp", fileName = "WallHackPowerUp")]

    public class WallHackPowerUp : CooldownPowerUp
    {
        public override void OnBeUnlocked(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            IsUnlocked = true;
            CanBeUsed = true;
        }

        public override void OnBeUsed(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            if (CanBeUsed && IsUnlocked)
            {
                playerEntity.StartCooldownCoroutine(this);
                foreach (Entities.Enemy enemy in EnemyManager.Instance.SpawnedEnemies)
                {
                    if (enemy.outline == null)
                    {
                        Transform found = enemy.transform.Find("OutlineObject");
                        if (found != null)
                        {
                            enemy.outline = found.gameObject;
                        }
                        else
                        {
                            Debug.LogError("OutlineObject introuvable dans " + enemy.name);
                            continue;
                        }
                    }
                    enemy.outline.SetActive(true);
                    playerEntity.StartCoroutine(CompetenceDuration(playerEntity, playerMovement, OnFinishedToBeUsed));

                }

                OnFinishedToBeUsed(playerEntity, playerMovement);
            }
            
        }

        public override void OnFinishedToBeUsed(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            foreach (Entities.Enemy enemy in EnemyManager.Instance.SpawnedEnemies)
            {
                if (enemy.outline != null)
                {
                    enemy.outline.SetActive(false);
                }
            }
            
            CanBeUsed = true;
        }
        
        public override void OnReset(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            OnFinishedToBeUsed(playerEntity, playerMovement);
            IsUnlocked = false;
        }
    }
}