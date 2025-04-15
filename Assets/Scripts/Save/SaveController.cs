using SaveSystem.Core;
using UnityEngine;

namespace RogueLike.Save
{
    public class SaveController : ISaveController<SampleSaveFile>
    {
        private const string SAMPLE_SAVE_FILE = "SaveFile";

        public SampleSaveFile GetDefaultSaveFile() => new SampleSaveFile()
        {
            currency = 0,
        };

        public bool Push(in SampleSaveFile saveFile)
        {
            string json = JsonUtility.ToJson(saveFile);
            PlayerPrefs.SetString(SAMPLE_SAVE_FILE, json);
            return true;
        }

        public bool Pull(out SampleSaveFile saveFile)
        {
            saveFile = default;
            if (!PlayerPrefs.HasKey(SAMPLE_SAVE_FILE))
                return false;

            string json = PlayerPrefs.GetString(SAMPLE_SAVE_FILE);
            saveFile = JsonUtility.FromJson<SampleSaveFile>(json);
            return true;
        }
    }
}