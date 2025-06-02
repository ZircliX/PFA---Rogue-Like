using DeadLink.Level.Interfaces;
using UnityEngine;

namespace RogueLike.Timer
{
    public struct TimerElementInfos : ILevelElementInfos
    {
        [SerializeField] public float timer;
    }
}