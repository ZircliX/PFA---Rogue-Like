using DG.Tweening;
using RogueLike.Player;
using UnityEngine;

namespace DeadLink.Extensions
{
    public static class PlayerExtensions
    {
        public static void TeleportPlayer(this PlayerMovement pm, Transform target, float extend = 0)
        {
            TeleportPlayer(pm, target.position, extend);
        }
        
        public static void TeleportPlayer(this PlayerMovement pm, Vector3 position, float extend = 0)
        {
            position.y += extend;
            
            pm.rb.MovePosition(position);
        }
    }
}