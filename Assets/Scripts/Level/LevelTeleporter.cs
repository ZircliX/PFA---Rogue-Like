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
            for (int i = 0; i <= 9; i++)
            {
                if (Input.GetKeyDown(KeyCode.Keypad0 + i))
                {
                    if (i < teleports.Length)
                    {
                        Teleport(teleports[i]);
                    }
                    break;
                }
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
