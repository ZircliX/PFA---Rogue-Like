using SaveSystem.Core;

namespace RogueLike.Save
{
    [System.Serializable]
    public struct SampleSaveFile : ISaveFile
    {
        public int Version { get; }

        public int currency;
    }
}