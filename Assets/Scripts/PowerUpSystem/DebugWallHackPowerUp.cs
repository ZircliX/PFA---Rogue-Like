using System;
using DeadLink.Entities;
using UnityEngine;

namespace DeadLink.PowerUpSystem
{
    public class DebugWallHackPowerUp : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                ActiveWallHack();
                Debug.Log("ActiveWallHack");
            }
            
            if (Input.GetKeyDown(KeyCode.N))
            {
                DesactiveWallHack();
                Debug.Log("DesactiveWallHack");
            }
        }

        public void ActiveWallHack()
        {
            foreach (Entities.Enemy enemy in EnemyManager.Instance.SpawnedEnemies)
            {
                if (enemy.outline == null)
                {
                    Transform found = enemy.transform.Find("OutlineObject");
                    if (found != null)
                    {
                        enemy.outline = found.gameObject;
                    }
                    else
                    {
                        Debug.LogError("OutlineObject introuvable dans " + enemy.name);
                        continue;
                    }
                }

                enemy.outline.SetActive(true);
            }
            
        }
        
        public void DesactiveWallHack()
        {
            foreach (Entities.Enemy enemy in EnemyManager.Instance.SpawnedEnemies)
            {
                if (enemy.outline != null)
                {
                    enemy.outline.SetActive(false);
                }
            }
        }
    }
}