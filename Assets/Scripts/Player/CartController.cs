using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using PathCreation;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class CartController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform graphicsObject;
    [SerializeField] private PathCreator path;
    [SerializeField] private HitboxManager hitboxManager;
    [SerializeField] private UnityEvent onSlide;
    [SerializeField] private PlayerInputsHolder playerInputsHolder;
    
    [Header("Animators")]
    [SerializeField] private Animator[] charactersAnimators;
    [SerializeField] private Animator cartAnimator;
    
    [Header("Cart Controls")]
    [SerializeField] private float cartMinSpeed = 1f;
    [SerializeField] private float cartMaxSpeed = 5f;
    [SerializeField] private float cartPumpAcceleration = 0.2f;
    [SerializeField] private float cartDeccelerationRate = 0.1f;

    [Header("Ramps Acceleration")] 
    [SerializeField] private AnimationCurve accelerationCurve;
    
    [Header("Others")]
    [SerializeField] private Quaternion rotationOffset = Quaternion.identity;
    private float distanceTravelled = 0f;

    [Header("Display info")]
    [SerializeField] private float cartSpeed = 0f;
    private float rampCoef = 1f;

    private int leanDirection = 0;
    public int LeanDirection => leanDirection;

    public float CartSpeed
    {
        get => cartSpeed;
        set
        {
            cartSpeed = value;
            cartSpeed = Mathf.Clamp(cartSpeed, cartMinSpeed, cartMaxSpeed);
        }
    }

    private void Awake()
    {
        hitboxManager.onHit += DeccelerateCart;
    }

    public void DeccelerateCart()
    {
        CartSpeed = cartMinSpeed;
    }

    public void AccelerateCart()
    {
        CartSpeed += cartPumpAcceleration;
    }

    private void FixedUpdate()
    {
        CartSpeed -= cartDeccelerationRate * Time.fixedDeltaTime;
    }

    private void OnGUI()
    {
#if UNITY_EDITOR
        //GUI.Label(new Rect(10, 10, 100, 20), $"Cart Speed: {cartSpeed}");
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInputsHolder.InputProviders[0] != null && playerInputsHolder.InputProviders[1] != null)
        {
            // int lean inputs
            int leanDir0 = playerInputsHolder.InputProviders[0].getLeanDirection();
            int leanDir1 = playerInputsHolder.InputProviders[1].getLeanDirection();
            
            // Debug.Log("LeanValue P1 : " + leanDir0 + " LeanValue P2 : " + leanDir1);
            if ((leanDir0 == leanDir1) && (leanDir0 != 0))
            {
                Lean(leanDir0);
            }
            else
            {
                Lean(0);
            }

            cartAnimator.SetInteger("LeanDirection", LeanDirection);

            
            // lean animation
            charactersAnimators[0].SetFloat("PUSH_PULL", playerInputsHolder.InputProviders[0].getLeanValue());
            charactersAnimators[1].SetFloat("PUSH_PULL", playerInputsHolder.InputProviders[1].getLeanValue());
        }
        
        distanceTravelled += Time.deltaTime * cartSpeed;
        transform.position = path.path.GetPointAtDistance(distanceTravelled);
        transform.rotation = path.path.GetRotationAtDistance(distanceTravelled) * rotationOffset;

        // Get direction at pos and determine angle
        Vector3 directionAtPos = path.path.GetDirectionAtDistance(distanceTravelled);
        float angleAtDistance = Vector3.Angle(-Vector3.up, directionAtPos);

        // offset angle so there's negative value for down ramp
        float rampDegree = angleAtDistance - 90f;
        
        rampCoef = accelerationCurve.Evaluate(rampDegree);
        // Debug.Log(rampDegree + " " + rampCoef);
    }
    public void Lean(int direction)
    {
        if (direction != leanDirection)
        {
            SetLeanState(direction);
        }
    }

    private void SetLeanState(int direction)
    {
        Debug.Log("LEAN ||| " + direction);
        
        hitboxManager.SetActiveHitbox((int) HitboxManager.Hitbox.DownRight, true);
        hitboxManager.SetActiveHitbox((int) HitboxManager.Hitbox.DownLeft, true);
        
        if (direction == -1)
        {
            // Leaning to left, disable left hibox
            hitboxManager.SetActiveHitbox((int) HitboxManager.Hitbox.DownRight, false);
            onSlide?.Invoke();
        }
        else if (direction == 1)
        {
            // Leaning to right, disable left hibox
            hitboxManager.SetActiveHitbox((int) HitboxManager.Hitbox.DownLeft, false);
            onSlide?.Invoke();

        }
        
        leanDirection = direction;
        // graphicsObject.transform.localEulerAngles = new Vector3(-40 * direction, 0,0);
    }
}
