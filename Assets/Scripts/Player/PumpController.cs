using System;
using UnityEngine;
using UnityEngine.Events;
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
    [SerializeField] [Range(0, 100f)] private float pumpInterruptionPercent = 95f;
    [SerializeField] private AnimationCurve minPumpTimeDegreeMultiplierCurve;
    [SerializeField] private AnimationCurve minPumpTimeSpeedMultiplierCurve;
    [SerializeField] private float pumpTooFast = .1f;
    [SerializeField] private float pumpWrongTurn = .1f;

    [Header("Animations")]
    [SerializeField] private AnimationCurve characterMovementCurve;

    [Header("Events")] 
    [SerializeField] public UnityEvent onPump;
    [SerializeField] public UnityEvent onWrongPump;
    [SerializeField] public UnityEvent onPumpTooFast;

    [Header("Display Info")]
    [SerializeField] private float currentMinPumpTime = 0f;

    private bool canPump = false;
    public bool CanPump => canPump;

    private float lastPumpTime;
    private float nextPumpCooldown;
    
    private int playerIndexTurn = 0;
    public int PlayerIndexTurn => playerIndexTurn;
    private float pumpEquilibrium = 1f;

    public void Update()
    {
        UpdatePumpMinTime();

        canPump = Time.time - lastPumpTime >= nextPumpCooldown * (pumpInterruptionPercent / 100);
        
        // HIGH LOW animation
        // Debug.Log(playerIndexTurn);
        // TODO : C'est pas de bon de faire ça mais faut trouver la bonne solution au lieu de * 4
        if (playerIndexTurn == 1) pumpEquilibrium += Time.deltaTime * (1 / currentMinPumpTime);
        else if (playerIndexTurn == 0) pumpEquilibrium -= Time.deltaTime * (1 / currentMinPumpTime);
        pumpEquilibrium = Mathf.Clamp(pumpEquilibrium, 0, 1);

        float pumpEquilibriumAnimation = characterMovementCurve.Evaluate(pumpEquilibrium);
        float pumpEquilibriumAnimationClamped = (pumpEquilibriumAnimation * 2) - 1;

        charactersAnimators[1].SetFloat("HIGH_LOW", -pumpEquilibriumAnimationClamped);
        charactersAnimators[0].SetFloat("HIGH_LOW", pumpEquilibriumAnimationClamped);
    }

    private void UpdatePumpMinTime()
    {
        currentMinPumpTime = minPumpTime * minPumpTimeSpeedMultiplierCurve.Evaluate(cartController.CartSpeed) * minPumpTimeDegreeMultiplierCurve.Evaluate(cartController.CurrentRampDegree);
        
        float pumpAnimationSpeedModifier = pumpAnimation.length / currentMinPumpTime;
        cartAnimator?.SetFloat("pumpSpeed", pumpAnimationSpeedModifier);
    }
    
    public void Pump(int playerIndex)
    {
        if(cartController.HasCartReachedEnd) return;
        
        if (playerIndex == playerIndexTurn)
        {
            if(cartController.LeanDirection != 0) return;
            
            if (!canPump)
            {
                Debug.Log("Pump too fast !");
                onPumpTooFast?.Invoke();
                cartController.CartSpeed -= pumpTooFast;
                return;
            }
            
            lastPumpTime = Time.time;
            nextPumpCooldown = currentMinPumpTime;
            
            onPump?.Invoke();
            SwitchPlayerIndex();
            cartController.AccelerateCart();
        }
        else
        {
            // Player who pressed pump button is not the one who should pump
            //Debug.Log("Wrong pump !");
            onWrongPump?.Invoke();
            cartController.CartSpeed -= pumpWrongTurn;
        }
    
    }

    private void SwitchPlayerIndex()
    {
        // alternate between 0 and 1
        playerIndexTurn = (playerIndexTurn + 1) % 2;
        if (cartAnimator != null)
        {
            cartAnimator.SetInteger("playerIndexTurn", playerIndexTurn);
            
        } else Debug.Log("mqlsdjf");

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
