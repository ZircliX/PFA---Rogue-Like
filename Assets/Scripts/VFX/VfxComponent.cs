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
        public void Initialize(Transform author)
        {
            transform.SetParent(author);
        }

        private void Update()
        {
            transform.position = cameraTransform.position;
            transform.rotation = cameraTransform.rotation;
            Debug.Log(transform.rotation.eulerAngles);
        }

        private void OnDisable()
        {
            //je suis la pour faire beau
        }
    }
}