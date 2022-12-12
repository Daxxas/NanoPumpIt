using System;
using System.Collections.Generic;
using UnityEngine;


public class CartVFX : MonoBehaviour
{
    [Header("VFX References")]
    [SerializeField] private List<ParticleSystem> leanLeft;
    [SerializeField] private List<ParticleSystem> leanRight;
    [SerializeField] private ParticleSystem speed;
    [SerializeField] private GameObject hitVFX;

    [Header("References")] 
    [SerializeField] private CartController cartController;

    [Header("Parameters")] 
    [SerializeField] private float speedThreshold = 3f;
    
    private void Start()
    {
        cartController.OnCartLean.AddListener(LeanVFX);
        cartController.OnCartLeanStop.AddListener(LeanVFX);
    }

    private void Update()
    {
        SpeedVFX();
    }

    public void HitVFX(Vector3 position)
    {
        Instantiate(hitVFX, position, Quaternion.identity);
    }
    
    private void LeanVFX()
    {   
        Debug.Log(cartController.LeanDirection);
        if (cartController.LeanDirection == -1)
        {
            foreach (var particle in leanLeft)
            {
                particle.Play(true);
            }
        }
        else if (cartController.LeanDirection == 1)
        {
            foreach (var particle in leanRight)
            {
                particle.Play(true);
            }
        }
        else
        {
            foreach (var particle in leanRight)
            {
                particle.Stop();
            }
            foreach (var particle in leanLeft)
            {
                particle.Stop();
            }
        }
    }

    
    private void SpeedVFX()
    {
        if(cartController.CartSpeed > speedThreshold)
        {
            // Avoid calling play every frame
            if (speed.isPlaying)
                return;
            
            speed.Play();
        }
        else
        {
            speed.Stop();
        }
    }
}