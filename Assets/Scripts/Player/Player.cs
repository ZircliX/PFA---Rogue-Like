using Enemy;
using LTX.ChanneledProperties;
using UnityEngine;

namespace RogueLike.Player
{
    public class Player : Entity
    {
        [SerializeField] private Vector3 SP;
        
        
        public override void Spawn(EntityData data, DifficultyData difficultyData, Vector3 SpawnPosition)
        {
            base.Spawn(data, difficultyData, SP);
            MaxHealth.AddInfluence(difficultyData, difficultyData.PlayerHealthMultiplier, Influence.Multiply);
            Strength.AddInfluence(difficultyData, difficultyData.PlayerStrengthMultiplier, Influence.Multiply);
            
            SetFullHealth();
        }
    }
}