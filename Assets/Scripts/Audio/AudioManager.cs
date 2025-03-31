using FMOD.Studio;
using FMODUnity;
using LTX.Singletons;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
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
