using DeadLink.PowerUp.Components;
using RogueLike.Entities;
using UnityEngine;

namespace DeadLink.PowerUp.ActivePowerUps
{
    [CreateAssetMenu(menuName = "PowerUp/InstantHealPowerUp", fileName = "InstantHealPowerUp")]

    public class InstantHealPowerUp : PowerUp
    {
        [field: SerializeField] public int InstantHealBonus { get; private set; } = 40;
        public override string Name { get; set; } = "InstantHeal";


        public override void OnBeUnlocked(VisitableComponent visitable)
        {
            isUnlocked = true;
            // 
        }

        public override void OnBeUsed(VisitableComponent visitable)
        {
            RogueLike.Entities.Player player = visitable as RogueLike.Entities.Player;
            if (player != null)
            {
                if (isUnlocked)
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