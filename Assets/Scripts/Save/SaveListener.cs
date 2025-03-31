using RogueLike.Controllers;
using SaveSystem.Core;

namespace RogueLike.Save
{
    public class SaveListener : ISaveListener<SampleSaveFile>
    {
        public SaveListener Global => GameController.SaveListener;
        public int Priority { get; }
        public int Currency { get; private set; }
        public int Served { get; private set; } = 0;
        public int Missed { get; private set; } = 0;
        public int Cooked { get; private set; } = 0;
    
    
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

        public void AddServed()
        {
            Served++;
        }

        public void AddMissed()
        {
            Missed++;
        }

        public void AddCooked()
        {
            Cooked++;
        }

        public void Reset()
        {
            Served = 0;
            Missed = 0;
            Cooked = 0;
        }
    }
}