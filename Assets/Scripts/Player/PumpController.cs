﻿using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PumpController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HitboxManager hitboxManager;
    [SerializeField] private CartController cartController;
    [SerializeField] private PlayerInputsHolder playerInputsHolder;
    [SerializeField] private AnimationClip pumpAnimation;

    [Header("Animators")]
    [SerializeField] private Animator[] charactersAnimators;
    [SerializeField] private Animator cartAnimator;

    [Header("Pump Settings")] 
    [SerializeField] private float minPumpTime = 0.1f;
    [SerializeField] private AnimationCurve minPumpTimeDegreeMultiplierCurve;
    [SerializeField] private AnimationCurve minPumpTimeSpeedMultiplierCurve;

    [Header("Display Info")]
    [SerializeField] private float currentMinPumpTime = 0f;
    
    private float lastPumpTime;
    
    private int playerIndexTurn = 0;
    private float pumpEquilibrium = 1f;

    public void Update()
    {
        UpdatePumpMinTime();
        
        // HIGH LOW animation
        // Debug.Log(playerIndexTurn);
        // TODO : Demander à Lucas pourquoi y'a un *4 ici
        if (playerIndexTurn == 1) pumpEquilibrium += Time.deltaTime * 4;
        else if (playerIndexTurn == 0) pumpEquilibrium -= Time.deltaTime * 4;
        pumpEquilibrium = Mathf.Clamp01(pumpEquilibrium);

        charactersAnimators[0].SetFloat("HIGH_LOW", 1-pumpEquilibrium);
        charactersAnimators[1].SetFloat("HIGH_LOW", pumpEquilibrium);
    }

    private void UpdatePumpMinTime()
    {
        currentMinPumpTime = minPumpTime * minPumpTimeSpeedMultiplierCurve.Evaluate(cartController.CartSpeed) * minPumpTimeDegreeMultiplierCurve.Evaluate(cartController.CurrentRampDegree);
        
        float pumpAnimationSpeedModifier = pumpAnimation.length / minPumpTime;
        cartAnimator.SetFloat("pumpSpeed", pumpAnimationSpeedModifier);
    }
    
    public void Pump(int playerIndex)
    {
        if (playerIndex == playerIndexTurn)
        {
            if (currentMinPumpTime > Time.time - lastPumpTime)
            {
                Debug.Log("Pump too fast !");
                return;
            }
            
            lastPumpTime = Time.time;
            
            SwitchPlayerIndex();
            cartController.AccelerateCart();
        }
        else
        {
            // Player who pressed pump button is not the one who should pump
            Debug.Log("Wrong pump !");
        }
    
    }

    private void SwitchPlayerIndex()
    {
        // alternate between 0 and 1
        playerIndexTurn = (playerIndexTurn + 1) % 2;
        cartAnimator.SetInteger("playerIndexTurn", playerIndexTurn);

        if (playerIndexTurn == 0)
        {
            hitboxManager.SetActiveHitbox((int) HitboxManager.Hitbox.Left, false);
            hitboxManager.SetActiveHitbox((int) HitboxManager.Hitbox.Right, true);
        }
        else
        {
            hitboxManager.SetActiveHitbox((int) HitboxManager.Hitbox.Left, true);
            hitboxManager.SetActiveHitbox((int) HitboxManager.Hitbox.Right, false);
        }
    }
}