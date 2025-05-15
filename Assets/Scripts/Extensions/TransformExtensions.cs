using UnityEngine;

namespace DeadLink.Extensions
{
    public static class TransformExtensions
    {
        public static SerializedTransform ToSerializeTransform(this Transform transform)
        {
            Quaternion rt = transform.rotation;
            return new SerializedTransform()
            {
                Position = transform.position,
                Rotation = new Vector4(rt.x, rt.y, rt.z, rt.w),
                Scale = transform.localScale
            };
        }
        
        public static void ApplySerialized(this Transform transform, SerializedTransform serializedTransform)
        {
            transform.position = serializedTransform.Position;
            transform.rotation = new Quaternion(serializedTransform.Rotation.x, serializedTransform.Rotation.y,
                serializedTransform.Rotation.z, serializedTransform.Rotation.w);
            transform.localScale = serializedTransform.Scale;
            
        }
    }
    
    [System.Serializable]
    public struct SerializedTransform
    {
        public Vector3 Position;
        public Vector4 Rotation;
        public Vector3 Scale;
    }
}