using SaveSystem.Core;
using UnityEngine;

namespace DeadLink.Save.GameProgression
{
    public class GameSaveController : ISaveController<GameProgression>
    {
        private const string GAME_PROGRESSION_SAVE_FILE = "GameProgressionSaveFile";

        public GameProgression GetDefaultSaveFile()
        {
            return GameProgression.GetDefault();
        }

        public bool Push(in GameProgression saveFile)
        {
            string json = JsonUtility.ToJson(saveFile);
            PlayerPrefs.SetString(GAME_PROGRESSION_SAVE_FILE, json);
            return true;
        }
        
        public bool Pull(out GameProgression saveFile)
        {
            saveFile = default;
            if (!PlayerPrefs.HasKey(GAME_PROGRESSION_SAVE_FILE))
                return false;

            string json = PlayerPrefs.GetString(GAME_PROGRESSION_SAVE_FILE);
            saveFile = JsonUtility.FromJson<GameProgression>(json);
            return true;
        }
    }
}