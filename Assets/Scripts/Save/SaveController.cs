using System.Collections.Generic;
using DeadLink.PowerUpSystem;
using SaveSystem.Core;
using UnityEngine;
using ZLinq;

namespace RogueLike.Save
{
    public class SaveController : ISaveController<GameProgression>
    {
        private const string SAMPLE_SAVE_FILE = "SaveFile";

        public GameProgression GetDefaultSaveFile() => new GameProgression()
        {
            DifficultyData = GameMetrics.Global.NormalDiffuculty,
            
            HealthPoints = 100,
            HealthBarCount = 3,
            
            LastCheckPoint = null,
            EnemyPositions = new List<Transform>(),
            
            RemainingPowerUps = GameMetrics.Global.PowerUps.AsValueEnumerable().ToList(),
            PlayerPowerUps = new List<PowerUp>(),
        };

        public bool Push(in GameProgression saveFile)
        {
            string json = JsonUtility.ToJson(saveFile);
            PlayerPrefs.SetString(SAMPLE_SAVE_FILE, json);
            return true;
        }

        public bool Pull(out GameProgression saveFile)
        {
            saveFile = default;
            if (!PlayerPrefs.HasKey(SAMPLE_SAVE_FILE))
                return false;

            string json = PlayerPrefs.GetString(SAMPLE_SAVE_FILE);
            saveFile = JsonUtility.FromJson<GameProgression>(json);
            return true;
        }
    }
}