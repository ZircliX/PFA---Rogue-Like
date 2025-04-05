using Enemy;
using LTX.Singletons;
using RogueLike.Timer;
using UnityEngine;

namespace RogueLike.Managers
{
    public class LevelManager : MonoSingleton<LevelManager>
    {
        [SerializeField] private DifficultyData difficulty;

        private void OnEnable()
        {
            EnemyManager.Instance.OnAllEnemiesDie += AllEnemiesAreDie;
        }

        private void OnDisable()
        {
            EnemyManager.Instance.OnAllEnemiesDie -= AllEnemiesAreDie;
        }

        private void AllEnemiesAreDie()
        {
            //Utilisable pour les rooms (on fait ce qu'on veux dedans)
            FinishLevel();
        }

        private void Start()
        {
            StartLevel();
        }
        
        public void StartLevel()
        {
            //Debug.Log("Start Level");
            EnemyManager.Instance.SpawnEnemies(difficulty);
            TimerManager.Instance.StartTimer();
            // PlayerManager qui fait spawn le player? Ou c'est le LevelManager Qui fait spawn Le joueur ?
        }

        public void FinishLevel()
        {
            TimerManager.Instance.PauseTimer();
            //TODO finir le level (avec affichage des 3 powers-up puis le chemin que le joueur peux prendre, ect...
            
        }
    }
}