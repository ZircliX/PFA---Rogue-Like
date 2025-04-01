using LTX.Singletons;
using UnityEngine;

namespace RogueLike.Player
{
    public class PlayerController : MonoSingleton<PlayerController>
    {
        [field: SerializeField] public Transform Orientation { get; private set; }
        [field: SerializeField] public Transform PlayerModel { get; private set; }
        
        
        [field: SerializeField] public PlayerMovementOLD PlayerMovementOld { get; private set; }
        [field: SerializeField] public PlayerMovement PlayerMovement { get; private set; }
    }
}