using UnityEngine;

namespace DeadLink.Weapons.Data
{
    [CreateAssetMenu(menuName = "RogueLike/Weapons/LaserWeaponData")]
    public class LaserWeaponData : WeaponData
    {
        [field: Header("Stats")]
        [SerializeField] private float shootRate;
        public override float ShootRate => shootRate;
        [SerializeField] private int maxAmmunition;
        public override int MaxAmmunition => maxAmmunition;
        [SerializeField] private float overHeatDuration;
        public override float ReloadTime => overHeatDuration;
        [field: SerializeField] public float LaserGainRate { get; private set; }
    }
}