using Enemy;
using LTX.ChanneledProperties;

namespace RogueLike.Player
{
    public class Player : Entity
    {
        public override void Spawn(EntityData data, DifficultyData difficultyData)
        {
            base.Spawn(data, difficultyData);
            MaxHealth.AddInfluence(difficultyData, difficultyData.PlayerHealthMultiplier, Influence.Multiply);
            Strength.AddInfluence(difficultyData, difficultyData.PlayerStrengthMultiplier, Influence.Multiply);
            
            SetFullHealth();
        }
    }
}