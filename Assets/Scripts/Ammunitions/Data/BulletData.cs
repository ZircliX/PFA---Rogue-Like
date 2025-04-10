#if UNITY_EDITOR
using LTX.Editor;
using UnityEditor;
#endif
using DeadLink.Cameras;
using UnityEngine;

namespace DeadLink.Ammunitions.Data
{
    public abstract class BulletData : ScriptableObject
    {
        [field: Header("Base Stats")]
        [field : SerializeField] public float BulletSpeed { get; private set; }
        [field : SerializeField] public float Damage { get; private set; }
        [field : SerializeField] public float MaxLifeCycle { get; private set; }
        
        [field: Header("VFX")]
        [field : SerializeField] public ParticleSystem HitVFX { get; private set; }
        [field : SerializeField] public CameraShakeComposite CameraShake { get; private set; }
        
        [field: Header("Prefab")]
        [field : SerializeField] public Bullet BulletPrefab { get; private set; }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (BulletPrefab && !Application.isPlaying)
            {
                using (SerializedObject serializedObject = new SerializedObject(BulletPrefab))
                {
                    SerializedProperty dataProperty =
                        serializedObject.FindBackingFieldProperty(nameof(Bullet.BulletData));
                    dataProperty.objectReferenceValue = this;
                    
                    serializedObject.ApplyModifiedProperties();
                }
            }
        }
#endif
    }
}