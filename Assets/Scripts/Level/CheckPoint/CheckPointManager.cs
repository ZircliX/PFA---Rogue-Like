using System;
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
        [SerializeField] 
        private CheckPointInfos[] checkPoints;
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

        private void OnValidate()
        {
            if(Application.isPlaying)
                return;
            
            if(checkPoints.Any(ctx => ctx.CheckPoint.gameObject.CompareTag("Respawn")))
                return;
            
            checkPoints[0].CheckPoint.tag = "Respawn";
        }

        public void SetCheckPoint(CheckPoint checkPoint)
        {
            bool pointInfos = GetCheckPointInfos(checkPoint, out CheckPointInfos checkPointInfos);
   
            if (checkPointInfos.CheckPointIndex < CurrentCheckPoint.CheckPointIndex) 
                return;
            //Debug.Log($"new : {checkPointInfos.CheckPointIndex}, current : {CurrentCheckPoint.CheckPointIndex}");
            
            CurrentCheckPoint = checkPointInfos;
            LevelManager.Instance.SaveCurrentLevelScenario();
        }
        
        private bool GetCheckPointInfos(CheckPoint checkPoint, out CheckPointInfos checkPointInfos)
        {
            checkPointInfos = checkPoints.FirstOrDefault(ctx => ctx.CheckPoint == checkPoint);
            return !checkPointInfos.IsDefault;
        }
        
        public bool TryGetCheckPoint(int index, out CheckPoint checkPoint)
        {
            checkPoint = null;
            if (index < 0 || index >= checkPoints.Length) 
                return false;
            
            checkPoint = checkPoints.FirstOrDefault(ctx => ctx.CheckPointIndex == index).CheckPoint;
            return checkPoint != null;
        }

        public Transform GetRespawn()
        {
            GameObject respawn = GameObject.FindWithTag("Respawn");
            if (respawn == null)
            {
                return transform;
            }
            return respawn.transform;
        }
        public bool HasCheckPoint() => CurrentCheckPoint.CheckPoint != null;
    }
    
    [System.Serializable]
    public struct CheckPointInfos
    {
        public CheckPoint CheckPoint;
        public int CheckPointIndex;

        public bool IsDefault => CheckPoint != default && CheckPointIndex != default;
    }
}