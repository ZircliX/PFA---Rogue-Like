using DG.Tweening;
using RogueLike.Player;
using UnityEngine;

namespace DeadLink.Extensions
{
    public static class PlayerExtensions
    {
        public static void TeleportPlayer(this PlayerMovement pm, Transform target, float speed = 0.5f, float extend = 0)
        {
            TeleportPlayer(pm, target.position, speed, extend);
        }
        
        public static void TeleportPlayer(this PlayerMovement pm, Vector3 position, float speed = 0.5f, float extend = 0)
        {
            position.y += extend;
            
            pm.rb.isKinematic = true;
            pm.transform.DOMove(position, speed).OnComplete(() =>
            {
                pm.rb.isKinematic = false;
            });
        }
    }
}