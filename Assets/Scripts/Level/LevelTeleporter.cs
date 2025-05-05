using DeadLink.Extensions;
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
                    if (i is 7 or 9) continue;
                    
                    if (i < teleports.Length)
                    {
                        player.TeleportPlayer(teleports[i]);
                    }
                    break;
                }
            }
        }
    }
}
