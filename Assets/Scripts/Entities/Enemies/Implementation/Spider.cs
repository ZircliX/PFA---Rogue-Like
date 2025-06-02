using RogueLike;
using UnityEngine;

namespace DeadLink.Entities.Enemies
{
    public class Spider : MeleeEnemy
    {
        protected override void Shoot()
        {
            if (CurrentWeapon != null && CurrentWeapon.CurrentReloadTime >= CurrentWeapon.WeaponData.ReloadTime)
            {
                GameObject objectToHit = null;
                Vector3 direction = player.transform.position - transform.position;
                Debug.DrawRay(BulletSpawnPoint.position, direction * 50, Color.red);

                if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 500, GameMetrics.Global.BulletRayCast))
                {
                    objectToHit = hit.collider.gameObject;
                }
                CurrentWeapon.Fire(this, direction, objectToHit);
                StartCoroutine(Die());
            }
            else
            {
                //Debug.LogError($"No equipped weapon for {gameObject.name}");
            }
        }
    }
}