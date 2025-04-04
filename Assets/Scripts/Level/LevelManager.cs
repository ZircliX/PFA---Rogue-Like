using Enemy;
using KBCore.Refs;
using UnityEngine;

namespace RogueLike.Managers
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private DifficultyData difficulty;

        [SerializeField, Scene] private Entity[] entities;
        private void OnValidate() => this.ValidateRefs();
        
        private void Start()
        {
            StartLevel();
        }
        
        public void StartLevel()
        {
            for (var i = 0; i < entities.Length; i++)
            {
                var entity = entities[i];
                entity.Spawn(entity.CurrentData, difficulty);
            }
        }
    }
}