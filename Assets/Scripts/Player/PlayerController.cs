using System.Collections.Generic;
using System.Linq;
using DeadLink.Extensions;
using DeadLink.Level.Interfaces;
using KBCore.Refs;
using RogueLike.Entities;
using RogueLike.Managers;
using RogueLike.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DeadLink.Player
{
    public class PlayerController : LevelElement
    {
        public class PlayerInfos : ILevelElementInfos
        {
            public List<string> PlayerPowerUps;

            public int HealthPoints;
            public int HealthBarCount;
            
            public SerializedTransform LastCheckPoint;
        }
        
        [field: SerializeField, Self] public PlayerInput PlayerInput { get; private set; }
        [field: SerializeField, Child] public PlayerEntity PlayerEntity { get; private set; }
        [field: SerializeField, Child] public PlayerMovement PlayerMovement { get; private set; }
        
        [field: SerializeField] public Transform LastCheckPoint {get; private set;}

        protected override void OnValidate()
        {
            base.OnValidate();
            this.ValidateRefs();
        }
        
        internal override ILevelElementInfos Pull()
        {
            return new PlayerInfos()
            {
                PlayerPowerUps = PlayerEntity.PowerUps.Select(ctx => ctx.GUID).ToList(),
                HealthPoints = PlayerEntity.Health,
                HealthBarCount = PlayerEntity.HealthBarCount,
                LastCheckPoint = LastCheckPoint.ToSerializeTransform()
            };
        }

        internal override void Push(ILevelElementInfos levelElementInfos)
        {
            if (levelElementInfos is PlayerInfos playerInfos)
            {
                PlayerEntity.SetInfos(playerInfos);
                transform.ApplySerialized(playerInfos.LastCheckPoint);
                
                PlayerEntity.Spawn(
                    PlayerEntity.EntityData, 
                    LevelManager.Instance.Difficulty, 
                    PlayerEntity.SpawnPosition.position);
            }
        }
    }
}