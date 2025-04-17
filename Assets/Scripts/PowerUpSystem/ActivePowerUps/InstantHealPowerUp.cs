using DeadLink.PowerUpSystem.InterfacePowerUps;
using UnityEngine;

namespace DeadLink.PowerUpSystem.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/InstantHealPowerUp", fileName = "InstantHealPowerUp")]

    public class InstantHealPowerUp : PowerUp
    {
        [field: SerializeField] public int InstantHealBonus { get; private set; } = 40;
        public override string Name { get; set; } = "InstantHeal";


        public override void OnBeUnlocked(IVisitable visitable)
        {
            IsUnlocked = true;
            // 
        }

        public override void OnBeUsed(IVisitable visitable)
        {
            RogueLike.Entities.Player player = visitable as RogueLike.Entities.Player;
            if (player != null)
            {
                if (IsUnlocked)
                {
                    player.SetInstantHeal(InstantHealBonus);
                    Debug.Log("Visitor accepted in HealthComponent");
                }
                else
                {
                    Debug.Log("PowerUp is not unlocked");
                }
            }
            
        }
    }
}