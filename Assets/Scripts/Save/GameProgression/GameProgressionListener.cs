using SaveSystem.Core;

namespace DeadLink.Save.GameProgression
{
    public class GameProgressionListener : ISaveListener<GameProgression>
    {
        public int Priority => 1;
        
        public string PlayerName { get; private set; }
        
        public void LoadDefault()
        {
            GameProgression defaultSave = GameProgression.GetDefault();
            
            PlayerName = defaultSave.PlayerName;
        }
        
        public void Write(ref GameProgression saveFile)
        {
            saveFile.PlayerName = PlayerName;
        }

        public void Read(in GameProgression saveFile)
        {
            //DifficultyData = saveFile.DifficultyData;
            PlayerName = saveFile.PlayerName;
            
            //RemainingPowerUps = saveFile.RemainingPowerUps;
            //PlayerPowerUps = saveFile.PlayerPowerUps;
        }
        
        public void SetPlayerName(string playerName) => PlayerName = playerName;
    }
}