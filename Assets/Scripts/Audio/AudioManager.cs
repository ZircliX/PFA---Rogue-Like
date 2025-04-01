using FMOD.Studio;
using FMODUnity;
using LTX.Singletons;
using RogueLike.Controllers;
using UnityEngine;

public class AudioManager
{
    public static AudioManager Global => GameController.AudioManager;
    
    public void PlayOneShot(EventReference sound, Vector3 position)
    {
        RuntimeManager.PlayOneShot(sound, position);
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }
}
