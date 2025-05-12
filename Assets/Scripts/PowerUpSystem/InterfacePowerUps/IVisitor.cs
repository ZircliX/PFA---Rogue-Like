using RogueLike.Player;
using UnityEngine;

namespace DeadLink.PowerUpSystem.InterfacePowerUps
{
    public interface IVisitor
    {
        public string Name { get; }
        public string Description { get; }
        public Sprite[] Icon { get; }
        public bool IsUnlocked { get; }
        
        public void OnReset(RogueLike.Entities.Player player, PlayerMovement playerMovement);
        public void OnBeUnlocked(RogueLike.Entities.Player player, PlayerMovement playerMovement);
        public void OnBeUsed(RogueLike.Entities.Player player, PlayerMovement playerMovement);
    }
}