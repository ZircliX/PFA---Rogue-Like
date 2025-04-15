using DeadLink.PowerUp.Components;
using RogueLike.Entities;
using UnityEngine;

namespace DeadLink.PowerUp.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/BonusHealthBarPowerUp", fileName = "BonusHealthBarPowerUp")]

    public class BonusHealthBarPowerUp : PowerUp
    {
        [field: SerializeField] public int BonusHealthBarCount { get; private set; } = 1;

        public override string Name { get; set; } = "BonusHealthBar";

        public override void OnBeUnlocked(VisitableComponent visitable)
        {
            Player player = visitable as Player;
            if (player != null)
            {
                if (player.HealthBarCount >= 3)
                {
                    player.SetFullHealth();
                }
                else
                {
                    player.SetBonusHealthBarCount(BonusHealthBarCount);
                }
                Debug.Log("Visitor accepted in HealthComponent");
            }
        }

        public override void OnBeUsed(VisitableComponent visitable)
        {
            
        }
    }
}