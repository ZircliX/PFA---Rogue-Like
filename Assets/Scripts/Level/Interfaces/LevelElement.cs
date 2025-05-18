using System;
using EditorAttributes;
using RogueLike.Managers;
using UnityEngine;

namespace DeadLink.Level.Interfaces
{
    public abstract class LevelElement : MonoBehaviour
    {
        [field: SerializeField, ReadOnly] public string GUID { get; private set; }

        protected virtual void OnValidate()
        {
            if (string.IsNullOrEmpty(GUID))
            {
                GUID = Guid.NewGuid().ToString();
            }
        }

        private void OnEnable()
        {
            if (LevelManager.HasInstance)
                LevelManager.Instance.LevelElements.Add(this);
        }

        private void OnDisable()
        {
            if (LevelManager.HasInstance)
                LevelManager.Instance.LevelElements.Remove(this);
        }

        internal abstract ILevelElementInfos Pull();
        internal abstract void Push(ILevelElementInfos levelElementInfos);
    }
}