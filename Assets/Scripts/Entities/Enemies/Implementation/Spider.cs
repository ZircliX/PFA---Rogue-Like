using System.Collections;
using RogueLike;
using UnityEngine;

namespace DeadLink.Entities.Enemies
{
    public class Spider : MeleeEnemy
    {
        private Vector3 direction;
        private GameObject objectToHit;
        
        protected override void Shoot()
        {
            if (CurrentWeapon != null && CurrentWeapon.CurrentReloadTime >= CurrentWeapon.WeaponData.ReloadTime)
            {
                objectToHit = null;
                direction = player.transform.position - transform.position;
                Debug.DrawRay(BulletSpawnPoint.position, direction * 50, Color.red);

                if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 500, GameMetrics.Global.BulletRayCast))
                {
                    objectToHit = hit.collider.gameObject;
                }
                StartCoroutine(Die());
            }
            else
            {
                //Debug.LogError($"No equipped weapon for {gameObject.name}");
            }
        }

        public override IEnumerator Die()
        {
            yield return new WaitForSeconds(1f);
            CurrentWeapon.Fire(this, direction, objectToHit);
            AudioManager.Global.PlayOneShot(GameMetrics.Global.FMOD_EnemyExplode, transform.position);
            yield return base.Die();
        }
    }
}