using System;
using DeadLink.PowerUpSystem.InterfacePowerUps;
using EditorAttributes;
using RogueLike.Player;
using UnityEngine;

namespace DeadLink.PowerUpSystem
{
    public abstract class PowerUp : ScriptableObject, IVisitor
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public Sprite[] Icon { get; private set; }

        public bool IsUnlocked { get; protected set; }
        public bool CanBeUsed { get; protected set; }
        
        [field: SerializeField, ReadOnly] public string GUID { get; private set; }

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(GUID))
            {
                GUID = Guid.NewGuid().ToString();
            }
        }

        public virtual void OnReset(RogueLike.Entities.Player player, PlayerMovement playerMovement)
        {
        }

        public abstract void OnBeUnlocked(RogueLike.Entities.Player player, PlayerMovement playerMovement);
        public abstract void OnBeUsed(RogueLike.Entities.Player player, PlayerMovement playerMovement);
        public abstract void OnFinishedToBeUsed(RogueLike.Entities.Player player, PlayerMovement playerMovement);

    }
}