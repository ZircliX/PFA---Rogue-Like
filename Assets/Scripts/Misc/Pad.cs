using DeadLink.Entities.Movement;
using EditorAttributes;
using UnityEngine;

namespace DeadLink.Misc
{
    public class Pad : MonoBehaviour
    {
        [field : SerializeField, DrawHandle(handleSpace: Space.Self)] public Vector3 PadDirection { get; private set; }
        
        [field : SerializeField, Range(0, 100)] public float PadXForce { get; private set; }
        [field : SerializeField, Range(0, 100)] public float PadYForce { get; private set; }
        [field : SerializeField, Range(0, 100)] public float PadZForce { get; private set; }
        
        private void OnTriggerEnter(Collider col)
        {
            if (col.TryGetComponent(out EntityMovement em))
            {
                em.EnterPad(this);
            }
        }
        
        private void OnTriggerExit(Collider col)
        {
            if (col.TryGetComponent(out EntityMovement em))
            {
                em.ExitPad();
            }
        }
    }
}