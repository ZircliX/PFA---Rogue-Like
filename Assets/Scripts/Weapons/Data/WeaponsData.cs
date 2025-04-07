using UnityEngine;

namespace DeadLink.Weapons.Data
{
    [CreateAssetMenu(menuName = "Dead Link/Weapons/Data")]
    public class WeaponsData : ScriptableObject 
    {
        [field : SerializeField] public float FireRate { get; private set; }
        [field : SerializeField] public float Damage { get; private set; }
        [field : SerializeField] public float ReloadDuration { get; private set; }
        [field : SerializeField] public int MagCapacity { get; private set; }
        [field : SerializeField] public Weapon Weapon { get; private set; }
        
    }
}