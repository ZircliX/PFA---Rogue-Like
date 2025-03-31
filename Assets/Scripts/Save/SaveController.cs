using SaveSystem.Core;
using UnityEngine;

namespace RogueLike.Save
{
    public class SaveController : ISaveController<SampleSaveFile>
    {
        private const string SAMPLESAVEFILE = "SaveFile";

        public SampleSaveFile GetDefaultSaveFile() => new SampleSaveFile()
        {
            currency = 0,
        };

        public bool Push(in SampleSaveFile saveFile)
        {
            string json = JsonUtility.ToJson(saveFile);
            PlayerPrefs.SetString(SAMPLESAVEFILE, json);
            return true;
        }

        public bool Pull(out SampleSaveFile saveFile)
        {
            saveFile = default;
            if (!PlayerPrefs.HasKey(SAMPLESAVEFILE))
                return false;

            string json = PlayerPrefs.GetString(SAMPLESAVEFILE);
            saveFile = JsonUtility.FromJson<SampleSaveFile>(json);
            return true;
        }
    }
}