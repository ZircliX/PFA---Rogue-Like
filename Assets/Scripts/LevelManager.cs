using Enemy;
using LTX.Singletons;
using UnityEngine;

namespace DefaultNamespace
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private DifficultyData difficulty;
        
        public void StartLevel()
        {
            var entities = GameObject.FindObjectsByType<Entity>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            for (var i = 0; i < entities.Length; i++)
            {
                var entity = entities[i];
                entity.Spawn(entity.CurrentData, difficulty);
            }
        }
    }
}