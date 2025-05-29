using FMODUnity;
using UnityEngine;

namespace RogueLike
{
    public partial class GameMetrics
    {
        [field: Header("Sounds")]
        [field: Header("Player")]
        [field: SerializeField] public EventReference FMOD_PlayerHandAttack { get; private set; }
        [field: SerializeField] public EventReference FMOD_PlayerAutomaticReload { get; private set; }
        [field: SerializeField] public EventReference FMOD_PlayerAutomaticShoot { get; private set; }
        [field: SerializeField] public EventReference FMOD_PlayerLaserOverHeat { get; private set; }
        [field: SerializeField] public EventReference FMOD_PlayerLaserShoot { get; private set; }
        [field: SerializeField] public EventReference FMOD_PlayerRocketReload { get; private set; }
        [field: SerializeField] public EventReference FMOD_PlayerRocketShoot { get; private set; }
        [field: SerializeField] public EventReference FMOD_PlayerRocketHit { get; private set; }

        [field: SerializeField] public EventReference FMOD_PlayerDie { get; private set; }
        [field: SerializeField] public EventReference FMOD_EnemyDie { get; private set; }
        [field: SerializeField] public EventReference FMOD_HitEnemy { get; private set; }
        [field: SerializeField] public EventReference FMOD_Rewind { get; private set; }
        [field: SerializeField] public EventReference FMOD_SlowMo { get; private set; }
        [field: SerializeField] public EventReference FMOD_GlassBreak { get; private set; }
        
        [field: SerializeField] public EventReference FMOD_UIClick { get; private set; }
        [field: SerializeField] public EventReference FMOD_UIHover { get; private set; }

        [field: Header("Voices")]
        [field: SerializeField] public EventReference FMOD_Echo6BonusSelected { get; private set; }
        [field: SerializeField] public EventReference FMOD_Echo6SelectBonus { get; private set; }
        [field: SerializeField] public EventReference FMOD_Echo6Welcome { get; private set; }
        [field: SerializeField] public EventReference FMOD_Echo6Intro { get; private set; }
        [field: SerializeField] public EventReference FMOD_Echo6FireAndReload { get; private set; }
        [field: SerializeField] public EventReference FMOD_Echo6Jump { get; private set; }
        [field: SerializeField] public EventReference FMOD_Echo6Move { get; private set; }
        [field: SerializeField] public EventReference FMOD_Echo6Outro { get; private set; }
        [field: SerializeField] public EventReference FMOD_Echo6Pad { get; private set; }
        [field: SerializeField] public EventReference FMOD_Echo6RegainHP { get; private set; }

        [field: Header("Musics")]
        [field: SerializeField] public EventReference FMOD_Level1 { get; private set; }
        [field: SerializeField] public EventReference FMOD_Level2 { get; private set; }
        [field: SerializeField] public EventReference FMOD_Level3 { get; private set; }
        [field: SerializeField] public EventReference FMOD_Level4 { get; private set; }
        [field: SerializeField] public EventReference FMOD_Level5 { get; private set; }
        [field: SerializeField] public EventReference FMOD_Level6 { get; private set; }
        [field: SerializeField] public EventReference FMOD_MainMenu { get; private set; }
        [field: SerializeField] public EventReference FMOD_Shop { get; private set; }
    }
}