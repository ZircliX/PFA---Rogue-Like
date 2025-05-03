using FMODUnity;
using UnityEngine;

namespace RogueLike
{
    public partial class GameMetrics
    {
        [field: Header("Sounds")]
        [field: Header("Player")]
        [field: SerializeField] public EventReference FMOD_PlayerAutomaticReload { get; private set; }
        [field: SerializeField] public EventReference FMOD_PlayerAutomaticShoot { get; private set; }
        [field: SerializeField] public EventReference FMOD_PlayerHandAttack { get; private set; }
        [field: SerializeField] public EventReference FMOD_PlayerLaserOverHeat { get; private set; }
        [field: SerializeField] public EventReference FMOD_PlayerLaserReload { get; private set; }
        [field: SerializeField] public EventReference FMOD_PlayerLaserShoot { get; private set; }
        [field: SerializeField] public EventReference FMOD_PlayerRocketReload { get; private set; }
        [field: SerializeField] public EventReference FMOD_PlayerRocketShoot { get; private set; }

        [field: SerializeField] public EventReference FMOD_PlayerDie { get; private set; }
        [field: SerializeField] public EventReference FMOD_PlayerHealthGain { get; private set; }
        [field: SerializeField] public EventReference FMOD_PlayerHealthLoss { get; private set; }

        [field: SerializeField] public EventReference FMOD_PlayerDash { get; private set; }
        [field: SerializeField] public EventReference FMOD_PlayerJump { get; private set; }
        [field: SerializeField] public EventReference FMOD_PlayerSlide { get; private set; }
        [field: SerializeField] public EventReference FMOD_PlayerWalk { get; private set; }

        [field: Header("Enemies")]
        [field: SerializeField] public EventReference FMOD_EnemiesAttack { get; private set; }
        [field: SerializeField] public EventReference FMOD_EnemiesDeath { get; private set; }

        [field: Header("World")]
        [field: SerializeField] public EventReference FMOD_WorldAmbiance { get; private set; }
        [field: SerializeField] public EventReference FMOD_WorldMusic1 { get; private set; }

        [field: Header("Levels")]
        [field: SerializeField] public EventReference FMOD_LevelEnd { get; private set; }
        [field: SerializeField] public EventReference FMOD_LevelStart { get; private set; }
        [field: SerializeField] public EventReference FMOD_LevelPortal { get; private set; }
    }
}