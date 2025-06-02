using System;
using DevLocker.Utils;
using EditorAttributes;
using FMODUnity;
using UnityEngine;

namespace DeadLink.Misc
{
    [CreateAssetMenu(fileName = "SceneData", menuName = "RogueLike/SceneData")]
    public class SceneData : ScriptableObject
    {
        [field: SerializeField] public SceneReference Scene { get; private set; }
        [field: SerializeField] public int ScoreboardSceneIndex { get; private set; }
        [field: SerializeField] public EventReference LevelMusic { get; private set; }
        [field: SerializeField, ReadOnly] public string GUID { get; private set; }

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(GUID))
            {
                GUID = Guid.NewGuid().ToString();
            }
        }
    }
}