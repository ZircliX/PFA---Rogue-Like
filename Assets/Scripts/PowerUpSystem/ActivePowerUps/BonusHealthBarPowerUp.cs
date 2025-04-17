using DeadLink.PowerUpSystem.InterfacePowerUps;
using UnityEngine;

namespace DeadLink.PowerUpSystem.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/BonusHealthBarPowerUp", fileName = "BonusHealthBarPowerUp")]

    public class BonusHealthBarPowerUp : PowerUp
    {
        [field: SerializeField] public int BonusHealthBarCount { get; private set; } = 1;

        public override string Name { get; set; } = "BonusHealthBar";
        

        public override void OnBeUnlocked(IVisitable visitable)
        {
            RogueLike.Entities.Player player = visitable as RogueLike.Entities.Player;
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

        public override void OnBeUsed(IVisitable visitable)
        {
            
        }
    }
}