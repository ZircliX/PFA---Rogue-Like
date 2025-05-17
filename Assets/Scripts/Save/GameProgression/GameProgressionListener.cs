using SaveSystem.Core;

namespace DeadLink.Save.GameProgression
{
    public class GameProgressionListener : ISaveListener<GameProgression>
    {
        public int Priority => 1;
        
        public string PlayerName { get; private set; }
        
        public void Write(ref GameProgression saveFile)
        {
            saveFile.PlayerName = PlayerName;
        }

        public void Read(in GameProgression saveFile)
        {
            PlayerName = saveFile.PlayerName;
        }
        
        public void SetPlayerName(string playerName) => PlayerName = playerName;
    }
}