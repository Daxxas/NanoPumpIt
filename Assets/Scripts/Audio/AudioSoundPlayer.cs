using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;


public class AudioSoundPlayer : MonoBehaviour
{
   [SerializeField] private EventReference soundRef;
   [SerializeField] private bool setupSoundOnStart = false;

   private EventInstance soundInstance;
   
   private void Start()
   {
       if (setupSoundOnStart)
       {
           SetupSoundInstance();
       }
   }

   private void SetupSoundInstance()
   {
       soundInstance = FMODUnity.RuntimeManager.CreateInstance(soundRef);
   }

   public void PlaySoundOneShot()
    {
        FMODUnity.RuntimeManager.PlayOneShot(soundRef);
    }

    public void StartSoundLoop()
    {
        soundInstance.start();
    }

    public void StopSoundLoop()
    {
        soundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}