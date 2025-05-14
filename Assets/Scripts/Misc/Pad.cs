using System;
using System.Collections.Generic;
using DeadLink.Cameras.Data;
using DeadLink.Entities.Movement;
using RogueLike.Player.States;
using UnityEditor;
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
    
    [CustomEditor(typeof(Pad))]
    public class PadEditor : Editor
    {
        private float stepTime = 0.2f;

        private void OnSceneGUI()
        {
            Pad pad = (Pad)target;

            DrawTrajectory(pad);
        }

        private void DrawTrajectory(Pad pad)
        {
            List<Vector3> points = new List<Vector3>();
            float currentTime = 0;
            Vector3 lastPoint = pad.transform.position;
            
            while (currentTime < pad.PadDuration)
            {
                Vector3 velocity = PadState.CalculateVelocity(pad, currentTime);
                Vector3 point = lastPoint + velocity * stepTime;
                points.Add(point);
                lastPoint = point;
                currentTime += stepTime;
            }
            
            Handles.DrawPolyLine(points.ToArray());
        }
    }
}