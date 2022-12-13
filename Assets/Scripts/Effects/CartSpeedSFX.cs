using System;
using UnityEngine;


public class CartSpeedSFX : MonoBehaviour
{
    [SerializeField] private CartController cartController;
    [SerializeField] private AudioParameterSetter audioParameterSetter;
    [SerializeField] private AudioSoundPlayer audioSoundPlayer;

    private bool hasStarted = false;
    
    private void Update()
    {
        audioParameterSetter.ApplyParameter(cartController.CartSpeed);

        if (cartController.CartSpeed <= 0)
        {
            Debug.Log("Stop sound");
            hasStarted = false;
            audioSoundPlayer.StopSoundLoop();
        }
        else if(!hasStarted)
        {
            Debug.Log("Start sound");
            hasStarted = true;
            audioSoundPlayer.StartSoundLoop();
        }
    }
}
