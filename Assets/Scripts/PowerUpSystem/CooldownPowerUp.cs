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
            yield return new WaitForSeconds(CooldownTime);
            CanBeUsed = true;
        }

        public virtual IEnumerator CompetenceDuration()
        {
            yield return new WaitForSeconds(CompetenceTime);
        }
        
    }
}