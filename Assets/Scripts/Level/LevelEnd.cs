using System;
using RogueLike.Managers;
using UnityEngine;

namespace DeadLink
{
    [RequireComponent(typeof(Collider))]
    public class LevelEnd : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                LevelManager.Instance.FinishLevel();
            }
        }
    }
}