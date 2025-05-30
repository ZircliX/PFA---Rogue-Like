using System;
using System.Collections.Generic;
using System.Linq;
using DeadLink.Extensions;
using DeadLink.Level.CheckPoint;
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
        [Serializable]
        public class PlayerInfos : ILevelElementInfos
        {
            public List<string> PlayerPowerUps;

            public float HealthPoints;
            public int HealthBarCount;
            
            public int LastCheckPoint;

            public bool IsNull() => HealthPoints == 0 && HealthBarCount == 0;

            public PlayerInfos GetDefault()
            {
                return new PlayerInfos()
                {
                    PlayerPowerUps = null,
                    HealthPoints = 100,
                    HealthBarCount = 3,
                    LastCheckPoint = 0
                };
            }
        }
        
        [field: SerializeField, Self] public PlayerInput PlayerInput { get; private set; }
        [field: SerializeField, Child] public PlayerEntity PlayerEntity { get; private set; }
        [field: SerializeField, Child] public PlayerMovement PlayerMovement { get; private set; }
        
        [field: SerializeField] public Transform LastCheckPoint {get; set;}

        protected override void OnValidate()
        {
            base.OnValidate();
            this.ValidateRefs();
        }
        
        internal override ILevelElementInfos Pull()
        {
            CheckPointManager manager = CheckPointManager.Instance;
            
            Debug.Log($"Pulling player controller");
            return new PlayerInfos()
            {
                PlayerPowerUps = PlayerEntity.PowerUps.Select(ctx => ctx.GUID).ToList(),
                HealthPoints = PlayerEntity.Health,
                HealthBarCount = PlayerEntity.HealthBarCount,
                LastCheckPoint = manager.HasCheckPoint() ? manager.CurrentCheckPoint.CheckPointIndex : 0
            };
        }

        internal override void Push(ILevelElementInfos levelElementInfos)
        {
            if (levelElementInfos is PlayerInfos playerInfos)
            {
                //Debug.Log($"Checkpoint = {playerInfos.LastCheckPoint}");
                
                PlayerEntity.Spawn(
                    PlayerEntity.EntityData, 
                    LevelManager.Instance.Difficulty, 
                    PlayerEntity.transform.position);
                
                PlayerEntity.SetInfos(playerInfos);
                
                //Debug.Log(playerInfos.LastCheckPoint);
                if (CheckPointManager.Instance.TryGetCheckPoint(playerInfos.LastCheckPoint, out CheckPoint checkPoint))
                {
                    //Debug.Log($"Weeee {LastCheckPoint}");
                    PlayerMovement.TeleportPlayer(checkPoint.transform, 1);
                }
                else
                {
                    //Debug.Log("Hooo, Spawned at spawn");
                    Transform respawn = CheckPointManager.Instance.GetRespawn();
                    PlayerMovement.TeleportPlayer(respawn, 1);
                }
            }
        }
    }
}