using System;
using LTX.ChanneledProperties;
using LTX.Singletons;
using UnityEngine;

namespace Enemy
{
    public class WaveManager : MonoSingleton<WaveManager>
    {
        [field: SerializeField] public InfluencedProperty<int> WaveBalance { get; private set; }

        public int RemainingEnemies { get; private set; }

        public bool AllEnemiesKilled => RemainingEnemies == 0;
        
        public event Action OnAllEnemiesDie;

        

        public void SetupWaveManager(DifficultyData difficulty)
        {
            WaveBalance = new InfluencedProperty<int>(difficulty.WaveBalanceMultiplier);
            WaveBalance.AddInfluence(difficulty, difficulty.WaveBalanceMultiplier, Influence.Multiply);
            StartWaveMode();
        }

        private void StartWaveMode()
        {

        }
    }
}