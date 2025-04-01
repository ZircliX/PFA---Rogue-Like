using FMODUnity;
using UnityEngine;

namespace RogueLike
{
    public partial class GameMetrics
    {
        [field: Header("Sounds")]
        [field: SerializeField]
        public EventReference test { get; private set; }
    }
}