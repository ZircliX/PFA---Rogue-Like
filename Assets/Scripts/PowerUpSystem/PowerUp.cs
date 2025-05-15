using System;
using System.Linq;
using DeadLink.PowerUpSystem.InterfacePowerUps;
using EditorAttributes;
using RogueLike;
using RogueLike.Controllers;
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

        public static PowerUp GetPowerUpFromGUID(string guid)
        {
            return GameDatabase.Global.GetPowerUp(guid);
        }
        
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(GUID))
            {
                GUID = Guid.NewGuid().ToString();
            }
        }

        public virtual void OnReset(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement)
        {
        }

        public abstract void OnBeUnlocked(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement);
        public abstract void OnBeUsed(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement);
        public abstract void OnFinishedToBeUsed(RogueLike.Entities.PlayerEntity playerEntity, PlayerMovement playerMovement);

    }
}