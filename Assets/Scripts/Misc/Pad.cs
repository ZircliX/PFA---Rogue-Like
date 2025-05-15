using System;
using DeadLink.Cameras.Data;
using DeadLink.Entities.Movement;
using UnityEngine;

namespace DeadLink.Misc
{
    public class Pad : MonoBehaviour
    {
        [field : SerializeField] public Vector3 PadDirection { get; private set; }
        
        [field : SerializeField] public float PadDuration;
        [field : SerializeField] public AnimationCurve PadCurve;
        
        [field: SerializeField] public CameraEffectData CameraEffectData { get; protected set; }

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