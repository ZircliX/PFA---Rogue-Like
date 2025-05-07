using System;
using System.Collections;
using RogueLike.Player;
using UnityEngine;

namespace DeadLink.PowerUpSystem
{
    public abstract class CooldownPowerUp : PowerUp
    {
        [field: SerializeField] public float CooldownTime { get; private set; }
        [field: SerializeField] public float CompetenceTime { get; private set; }
        public virtual IEnumerator Cooldown()
        {
            CanBeUsed = false;
            float currentTime = 0;

            while (currentTime < CooldownTime)
            {
                currentTime += Time.deltaTime;
                HandleUI();
                
                yield return null;
            }
            
            CanBeUsed = true;
        }
        
        private void HandleUI()
        {
            //implement UI logic here?
        }

        public virtual IEnumerator CompetenceDuration(RogueLike.Entities.Player player, PlayerMovement playerMovement, Action<RogueLike.Entities.Player, PlayerMovement> callback)
        {
            float currentTime = 0;

            while (currentTime < CooldownTime)
            {
                currentTime += Time.deltaTime;
                
                yield return null;
            }
            
            callback.Invoke(player, playerMovement);

        }
        
    }
}