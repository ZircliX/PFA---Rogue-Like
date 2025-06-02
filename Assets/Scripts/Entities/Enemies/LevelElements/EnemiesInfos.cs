using DeadLink.Extensions;
using DeadLink.Level.Interfaces;
using UnityEngine;

namespace DeadLink.Entities.Enemies
{
    [System.Serializable]
    public struct EnemiesInfos : ILevelElementInfos
    {
        [SerializeField] public EnemyInfos[] Enemies; 
    }

    [System.Serializable]
    public struct EnemyInfos
    {
        [SerializeField] public string GUID;
        [SerializeField] public string EntityDataGUID;
        
        [SerializeField] public SerializedTransform Transform;
    }
}