using Enemy;
using LTX.ChanneledProperties;
using UnityEngine;

namespace RogueLike.Player
{
    public class Player : Entity
    {
        [SerializeField] private Vector3 spawnPosition;
        
        public override void Spawn(EntityData data, DifficultyData difficultyData, Vector3 spawnPoint)
        {
            base.Spawn(data, difficultyData, spawnPoint);
            MaxHealth.AddInfluence(difficultyData, difficultyData.PlayerHealthMultiplier, Influence.Multiply);
            Strength.AddInfluence(difficultyData, difficultyData.PlayerStrengthMultiplier, Influence.Multiply);
            
            SetFullHealth();
        }
    }
}