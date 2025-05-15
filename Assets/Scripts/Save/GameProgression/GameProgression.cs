using SaveSystem.Core;
using UnityEngine;

namespace DeadLink.Save.GameProgression
{
    [System.Serializable]
    public struct GameProgression : ISaveFile
    {
        public int Version => 1;
        
        [SerializeField] public string PlayerName;

        public static GameProgression GetDefault()
        {
            return new GameProgression()
            {
                PlayerName = "Unnamed",
            };
        }
    }
}