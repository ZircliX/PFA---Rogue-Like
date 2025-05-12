using DeadLink.PowerUpSystem.InterfacePowerUps;
using RogueLike;
using UnityEngine.InputSystem;

namespace DeadLink.Entities.Enemies
{
    public class DistanceEnemy : Enemy
    {
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
        }

        public override void Unlock(IVisitor visitor)
        {
        }

        public override void UsePowerUp(InputAction.CallbackContext context)
        {
        }

        protected override void Attack()
        {
            if (!canAttack) return;
            Shoot();
        }

        protected override void Shoot()
        {
            base.Shoot();
            AudioManager.Global.PlayOneShot(GameMetrics.Global.FMOD_EnemiesAttack, transform.position);
        }
    }
}