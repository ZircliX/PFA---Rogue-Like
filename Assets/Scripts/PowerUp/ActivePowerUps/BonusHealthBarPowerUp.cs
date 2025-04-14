using DeadLink.PowerUp.Components;
using RogueLike.Entities;
using UnityEngine;

namespace DeadLink.PowerUp.ActivePowerUps
{
    
    public class BonusHealthBarPowerUp : PowerUp
    {
        [field: SerializeField] public int BonusHealthBarCount { get; private set; } = 1;

        public override void OnBeUnlocked(VisitableComponent visitable)
        {
            Player player = visitable as Player;
            if (player != null)
            {
                if (player.healthBarCount >= 3)
                {
                    player.health = 100;
                }
                else
                {
                    player.healthBarCount += BonusHealthBarCount;
                }
                Debug.Log("Visitor accepted in HealthComponent");
            }
        }

        public override void OnBeUsed(VisitableComponent visitable)
        {
        }
    }
}