using System.Collections.Generic;
using DeadLink.PowerUpSystem;
using Enemy;
using SaveSystem.Core;
using ZLinq;

namespace DeadLink.Save.GameProgression
{
    public class GameProgressionListener : ISaveListener<GameProgression>
    {
        public int Priority => 1;
        
        public DifficultyData DifficultyData { get; private set; }
        public string PlayerName { get; private set; }

        public int LevelIndex { get; private set; }
        
        public List<PowerUp> RemainingPowerUps { get; private set; }
        public List<PowerUp> PlayerPowerUps { get; private set; }
        
        public void Write(ref GameProgression saveFile)
        {
            saveFile.DifficultyData = DifficultyData.GUID;
            saveFile.PlayerName = PlayerName;
            
            saveFile.RemainingPowerUps = RemainingPowerUps.AsValueEnumerable().Select(up => up.GUID).ToList();
            saveFile.PlayerPowerUps = PlayerPowerUps.AsValueEnumerable().Select(up => up.GUID).ToList();
        }

        public void Read(in GameProgression saveFile)
        {
            //DifficultyData = saveFile.DifficultyData;
            PlayerName = saveFile.PlayerName;
            
            //RemainingPowerUps = saveFile.RemainingPowerUps;
            //PlayerPowerUps = saveFile.PlayerPowerUps;
        }
        
        public void SetDifficultyData(DifficultyData data) => DifficultyData = data;

        public void SetPlayerName(string playerName) => PlayerName = playerName;

        public void SetLevelIndex(int levelIndex) => LevelIndex = levelIndex;

        public void SetRemainingPowerUps(List<PowerUp> remainingPowerUps) => RemainingPowerUps = remainingPowerUps;

        public void SetPlayerPowerUps(List<PowerUp> playerPowerUps) => PlayerPowerUps = playerPowerUps;
    }
}