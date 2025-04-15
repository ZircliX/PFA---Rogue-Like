using RogueLike.Controllers;
using SaveSystem.Core;

namespace RogueLike.Save
{
    public class SaveListener : ISaveListener<SampleSaveFile>
    {
        public SaveListener Global => GameController.SaveListener;
        public int Priority { get; }
        public int Currency { get; private set; }
        
        public void Write(ref SampleSaveFile saveFile)
        {
            saveFile.currency = Currency;
        }

        public void Read(in SampleSaveFile saveFile)
        {
            Currency = saveFile.currency;
        }

        public void AddCurrency(int amount)
        {
            Currency += amount;
        }

        public void SubCurrency(int amount)
        {
            Currency -= amount;
        }
    }
}