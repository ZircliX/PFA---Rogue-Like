using System;
using RogueLike.Managers;
using UnityEngine;
using UnityEngine.VFX;

namespace RogueLike.VFX
{
    public class VfxComponent : MonoBehaviour
    {
        [field: SerializeField] public ParticleSystem[] vfxPsArray { get; private set; }
        [field: SerializeField] public VisualEffect[] vfxVeArray { get; private set; }

        private Transform cameraTransform => LevelManager.Instance.PlayerController.PlayerMovement.Camera.transform;
        private bool camVFX = false;
        
        public void Initialize(Transform author)
        {
            transform.SetParent(author);
            camVFX = true;
        }

        private void Update()
        {
            if (camVFX)
            {
                transform.position = cameraTransform.position;
                transform.rotation = cameraTransform.rotation;
            }
        }

        private void OnDisable()
        {
            //je suis la pour faire beau
        }
    }
}