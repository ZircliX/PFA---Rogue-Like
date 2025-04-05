using System;
using Enemy;
using KBCore.Refs;
using RogueLike.Controllers;
using UnityEngine;

namespace RogueLike.Managers
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private DifficultyData difficulty;

        
        private void OnValidate() => this.ValidateRefs();

        private void OnEnable()
        {
            EnemiesManager.Instance.OnAllEnemiesDie += AllEnemiesAreDie;
        }

        private void OnDisable()
        {
            EnemiesManager.Instance.OnAllEnemiesDie -= AllEnemiesAreDie;

        }

        private void AllEnemiesAreDie()
        {
            //Utilisable pour les rooms (on fait ce qu'on veux dedans)
        }

        private void Start()
        {
            StartLevel();
        }
        
        public void StartLevel()
        {
            EnemiesManager.Instance.SpawnEnemies(difficulty);
            GameController.Timer.StartTimer();
            // PlayerManager qui fait spawn le player? Ou c'est le LevelManager Qui fait spawn Le joueur ?
        }

        public void FinishLevel()
        {
            GameController.Timer.PauseTimer();
            //TODO finir le level (avec affichage des 3 powers-up puis le chemin que le joueur peux prendre, ect...
            
        }
    }
}