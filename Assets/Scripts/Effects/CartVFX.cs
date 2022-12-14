using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CartVFX : MonoBehaviour
{
    [Header("VFX References")]
    [SerializeField] private List<ParticleSystem> leanLeft;
    [SerializeField] private List<ParticleSystem> leanRight;
    [SerializeField] private ParticleSystem dust;
    [SerializeField] private ParticleSystem speed;
    [SerializeField] private GameObject lever;
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private GameObject goutteVFX;

    [Header("References")] 
    [SerializeField] private CartController cartController;
    [SerializeField] private PumpController pumpController;
    [SerializeField] private Transform leverFXLeft;
    [SerializeField] private Transform leverFXRight;
    [SerializeField] private Transform goutteFXLeft;
    [SerializeField] private Transform goutteFXRight;

    [Header("Parameters")] 
    [SerializeField] private float speedThreshold = 3f;

    [Header("Events for SFX parce que j'ai la flemme")] 
    [SerializeField] private UnityEvent onSpeed;
    [SerializeField] private UnityEvent onSpeedStop;

    private void Start()
    {
        cartController.OnCartLean.AddListener(LeanVFX);
        cartController.OnCartLeanStop.AddListener(LeanVFX);
        pumpController.onPump.AddListener(PumpFX);
        pumpController.onWrongPump.AddListener(WrongPumpVFX);
    }

    private void Update()
    {
        SpeedVFX();

        var emissionDust = dust.emission;
        emissionDust.rateOverTime = Mathf.Clamp((float)(cartController.CartSpeed * 2.5) - 5, 0, 30);

        var emissionSpeed = speed.emission;
        emissionSpeed.rateOverTime = Mathf.Clamp((float)(cartController.CartSpeed * 1) - 5, 0, 30);

    }

    public void HitVFX(Vector3 position)
    {
        Instantiate(hitVFX, position, Quaternion.identity);
    }

    public void WrongPumpVFX()
    {
        if (pumpController.PlayerIndexTurn == 0)
        {
            Instantiate(goutteVFX, goutteFXLeft.transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(goutteVFX, goutteFXRight.transform.position, Quaternion.identity);
        }
    }
    
    public void PumpFX()
    {
        if (pumpController.PlayerIndexTurn == 0)
        {
            Instantiate(lever, leverFXLeft.position, Quaternion.identity);
        }else
        {
            Instantiate(lever, leverFXRight.position, Quaternion.identity);
        }
        
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
            onSpeed?.Invoke();
        }
        else
        {
            speed.Stop();
            onSpeedStop?.Invoke();
        }
    }
}