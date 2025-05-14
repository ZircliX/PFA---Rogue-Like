using System.Collections.Generic;
using RogueLike;
using SaveSystem.Core;
using UnityEngine;
using ZLinq;

namespace DeadLink.Save.GameProgression
{
    public class GameSaveController : ISaveController<GameProgression>
    {
        private const string GAME_PROGRESSION_SAVE_FILE = "GameProgressionSaveFile";

        public GameProgression GetDefaultSaveFile()
        {
            return new GameProgression()
            {
                DifficultyData = GameMetrics.Global.NormalDiffuculty.GUID,
                PlayerName = "Unnamed",
                RemainingPowerUps = GameMetrics.Global.PowerUps.AsValueEnumerable().Select(up => up.GUID).ToList(),
                PlayerPowerUps = new List<string>(),
            };
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