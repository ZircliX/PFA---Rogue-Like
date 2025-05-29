using System;
using FMOD.Studio;
using FMODUnity;
using LTX.Singletons;
using RogueLike.Controllers;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

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

    public void StopSounds()
    {
        try
        {
            RuntimeManager.GetBus("bus:/Music").stopAllEvents(STOP_MODE.ALLOWFADEOUT);
            RuntimeManager.GetBus("bus:/Voices").stopAllEvents(STOP_MODE.ALLOWFADEOUT);
            RuntimeManager.GetBus("bus:/SFX").stopAllEvents(STOP_MODE.ALLOWFADEOUT);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void StopVoices()
    {
        try
        {
            RuntimeManager.GetBus("bus:/Voices").stopAllEvents(STOP_MODE.ALLOWFADEOUT);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
