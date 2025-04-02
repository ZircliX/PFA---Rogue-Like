using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using KBCore.Refs;
using LTX.Tools;
using UnityEngine;
using ZLinq;

namespace RogueLike.Player
{
    public class PlayerRewind : MonoBehaviour
    {
        [SerializeField] private int maxComposites = 100;
        [SerializeField] private float compositesSpacing = 0.05f;
        [SerializeField] private float rewindSpeed = 1;
        private float currentCompositeTimer;
        private bool isRewinding;
        
        public List<PlayerRewindComposite> Composites { get; private set; }
        private DynamicBuffer<PlayerRewindComposite> buffer;
        public PlayerRewindComposite[] gogogo;
        
        [SerializeField, Self] private PlayerMovement pm;
        [SerializeField] private Transform camPivot;
        [SerializeField] private PlayerCamera cineMachineCam;
        Transform target => pm.transform;

        private void OnValidate() => this.ValidateRefs();

        private void Awake()
        {
            Composites = new List<PlayerRewindComposite>();
            buffer = new DynamicBuffer<PlayerRewindComposite>(32);
        }
        
        public IEnumerator DoRewind()
        {
            isRewinding = true;
            pm.enabled = false;
            
            bool originalKinematicState = false;
            originalKinematicState = pm.rb.isKinematic;
            pm.rb.isKinematic = true;


            buffer.Clear();
            buffer.CopyFrom(Composites);
            cineMachineCam.enabled = false;
            
            //cam.DORotate(Composites[0].Rotation, rewindSpeed * compositesSpacing * Composites.Count).SetEase(Ease.Linear);

            for (int i = buffer.Length - 1; i >= 0; i--)
            {
                PlayerRewindComposite currentComposite = buffer[i];

                target.DOKill();
                camPivot.DOKill();

                target.DOMove(currentComposite.Position, rewindSpeed * compositesSpacing).SetEase(Ease.Linear);
                camPivot.DOLocalRotate(currentComposite.Rotation, rewindSpeed * compositesSpacing).SetEase(Ease.Linear);

                //pm.health = currentComposite.Health;

                yield return new WaitForSeconds(rewindSpeed * compositesSpacing);
            }
            
            if (buffer.Length > 0)
            {
                 target.position = buffer[0].Position;
                 camPivot.rotation = Quaternion.Euler(buffer[0].Rotation);
                 pm.SetMovementState(buffer[0].State);
            }
            
            cineMachineCam.enabled = true;
            pm.enabled = true;
            pm.rb.isKinematic = originalKinematicState;
            isRewinding = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(DoRewind());
            }

            gogogo = Composites.AsValueEnumerable().ToArray();

            if (!isRewinding)
            {
                currentCompositeTimer += Time.deltaTime;
                if (currentCompositeTimer >= compositesSpacing)
                {
                    currentCompositeTimer = 0;

                    if (Composites.Count >= maxComposites)
                    {
                        Composites.RemoveAt(0);
                    }
                    
                    PlayerRewindComposite composite = new PlayerRewindComposite()
                    {
                        Position = target.position,
                        Rotation = camPivot.rotation.eulerAngles,
                        Velocity = pm.CurrentVelocity,
                        
                        State = pm.CurrentState,

                        Health = 0,
                    };

                    Composites.Add(composite);
                }
            }
        }
    }
    
    [System.Serializable]
    public struct PlayerRewindComposite : IEquatable<PlayerRewindComposite>
    {
        //Player Transform data
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Velocity;

        public MovementState State;
        
        //Player Stats data
        public float Health;

        public bool Equals(PlayerRewindComposite other)
        {
            return Position.Equals(other.Position) && Rotation.Equals(other.Rotation) && Velocity.Equals(other.Velocity) && State == other.State && Health.Equals(other.Health);
        }

        public override bool Equals(object obj)
        {
            return obj is PlayerRewindComposite other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Position, Rotation, Velocity, (int)State, Health);
        }
    }
}