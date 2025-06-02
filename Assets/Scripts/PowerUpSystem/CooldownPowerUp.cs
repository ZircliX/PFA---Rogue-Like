using System;
using System.Collections;
using DeadLink.Menus;
using RogueLike.Player;
using UnityEngine;

namespace DeadLink.PowerUpSystem
{
    public abstract class CooldownPowerUp : PowerUp
    {
        [field: SerializeField] public float CooldownTime { get; private set; }
        [field: SerializeField] public float CompetenceTime { get; private set; }
        
        public override void OnReset(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
            OnFinishedToBeUsed(playerEntity, playerMovement);
        }
        
        public virtual IEnumerator Cooldown()
        {
            CanBeUsed = false;
            float currentTime = 0;

            while (currentTime < CooldownTime)
            {
                currentTime += Time.unscaledDeltaTime;
                MenuManager.Instance.HUDMenu.UsePowerUp(this, currentTime, CooldownTime);
                
                yield return null;
            }
            
            CanBeUsed = true;
        }
        
        public virtual IEnumerator CompetenceDuration(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement, Action<RogueLike.Entities.PlayerEntity, PlayerMovement> callback)
        {
            float currentTime = 0;

            while (currentTime < CompetenceTime)
            {
                currentTime += Time.unscaledDeltaTime;
                
                yield return null;
            }
            
            callback.Invoke(playerEntity, playerMovement);
        }
    }
}