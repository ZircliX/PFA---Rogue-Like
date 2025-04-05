using DG.Tweening;
using KBCore.Refs;
using RogueLike.Player;
using UnityEngine;

namespace DeadLink
{
    public class LevelTeleporter : MonoBehaviour
    {
        [SerializeField, Child] private Transform[] teleports;
        [SerializeField] private PlayerMovement player;

        private void OnValidate() => this.ValidateRefs();
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                Teleport(teleports[0]);
            }
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                Teleport(teleports[1]);
            }
            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                Teleport(teleports[2]);
            }
            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                Teleport(teleports[3]);
            }
            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                Teleport(teleports[4]);
            }
            if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                Teleport(teleports[5]);
            }
            if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                Teleport(teleports[6]);
            }
            if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                Teleport(teleports[7]);
            }
            if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                Teleport(teleports[8]);
            }
            if (Input.GetKeyDown(KeyCode.Keypad9))
            {
                Teleport(teleports[9]);
            }
        }

        private void Teleport(Transform teleport)
        {
            player.rb.isKinematic = true;
            player.transform.DOMove(teleport.position, 0.25f).OnComplete(() =>
            {
                player.rb.isKinematic = false;
            });
        }
    }
}
