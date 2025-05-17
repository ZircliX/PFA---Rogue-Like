using System.Linq;
using DeadLink.Entities.Movement;
using DeadLink.Extensions;
using DG.Tweening;
using LTX.Singletons;
using RogueLike.Managers;
using RogueLike.Player;
using UnityEngine;

namespace DeadLink.Level.CheckPoint
{
    public class CheckPointManager : MonoSingleton<CheckPointManager>
    {
        [SerializeField] private CheckPointInfos[] checkPoints;
        public CheckPointInfos CurrentCheckPoint { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            CurrentCheckPoint = new CheckPointInfos()
            {
                CheckPoint = null,
                CheckPointIndex = 0
            };
        }

        public void TeleportToCheckPoint(PlayerMovement player)
        {
            if (CurrentCheckPoint.CheckPoint != null)
            {
                player.TeleportPlayer(CurrentCheckPoint.CheckPoint.transform, 1f);
            }
            else
            {
                player.TeleportPlayer(LevelManager.Instance.PlayerController.PlayerEntity.SpawnPosition, 1f);
            }
        }
        
        public void SetCheckPoint(CheckPoint checkPoint)
        {
            bool pointInfos = GetCheckPointInfos(checkPoint, out CheckPointInfos checkPointInfos);
            if (CurrentCheckPoint.CheckPoint != null && pointInfos)
            {
                if (checkPointInfos.CheckPointIndex < CurrentCheckPoint.CheckPointIndex) return;
            }
            CurrentCheckPoint = checkPointInfos;
            LevelManager.Instance.SaveCurrentLevelScenario();
        }
        
        private bool GetCheckPointInfos(CheckPoint checkPoint, out CheckPointInfos checkPointInfos)
        {
            checkPointInfos = checkPoints.FirstOrDefault(ctx => ctx.CheckPoint == checkPoint);
            return !checkPointInfos.IsDefault;
        }
    }
    
    [System.Serializable]
    public struct CheckPointInfos
    {
        public CheckPoint CheckPoint;
        public int CheckPointIndex;

        public bool IsDefault => CheckPoint != default && CheckPointIndex != default;
    }
}