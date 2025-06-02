using System.Collections;
using DeadLink.Entities;
using DeadLink.Weapons.Data;
using RogueLike;
using RogueLike.Controllers;
using RogueLike.Managers;
using RogueLike.VFX;
using UnityEngine;

namespace DeadLink.Weapons.WeaponsClass
{
    public class RocketLauncherWeapon : Weapon
    {
        public override WeaponData WeaponData => rocketWeaponData;
        [SerializeField] private RocketWeaponData rocketWeaponData;
        [SerializeField] private VfxComponent laserVfx;
        public override int CurrentMunitions { get; protected set; }
        public override float CurrentReloadTime { get; protected set; }

        public override bool Fire(Entity entity, Vector3 direction, GameObject shouldHit)
        {
            if (base.Fire(entity, direction, shouldHit))
            {
                AudioManager.Global.PlayOneShot(GameMetrics.Global.FMOD_PlayerRocketShoot, entity.transform.position);
                if (laserVfx != null)
                {
                    laserVfx.PlayVFX(LevelManager.Instance.PlayerController.ArmFlashPosition.position, LevelManager.Instance.PlayerController.delayAfterDestroyFlash);
                }
                return true;
            }

            return false;
        }

        public override IEnumerator Reload(Entity entity)
        {
            return base.Reload(entity);
        }
    }
}