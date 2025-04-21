#if UNITY_EDITOR
using LTX.Editor;
using UnityEditor;
#endif
using DeadLink.Cameras;
using UnityEngine;
using UnityEngine.VFX;

namespace DeadLink.Weapons.Data
{
    public abstract class WeaponData : ScriptableObject
    {
        [field: Header("VFX")]
        [field : SerializeField] public VisualEffect ShootVFX { get; private set; }
        [field : SerializeField] public CameraShakeComposite CameraShake { get; private set; }
        
        [field: Header("Shoot Stats")]
        [field : SerializeField] public float ShootRate { get; private set; }
        [field : SerializeField] public int MaxAmmunition { get; private set; }
        
        
        [field: Header("Prefab")]
        [field : SerializeField] public Weapon WeaponPrefab { get; private set; }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (WeaponPrefab && !Application.isPlaying)
            {
                using (SerializedObject serializedObject = new SerializedObject(WeaponPrefab))
                {
                    SerializedProperty dataProperty =
                        serializedObject.FindBackingFieldProperty(nameof(Weapon.WeaponData));
                    dataProperty.objectReferenceValue = this;
                    
                    serializedObject.ApplyModifiedProperties();
                }
            }
        }
#endif
    }
}