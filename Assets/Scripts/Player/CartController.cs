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
    [SerializeField] private PlayerInputsHolder playerInputsHolder;
    
    [Header("Animators")]
    [SerializeField] private Animator[] charactersAnimators;
    [SerializeField] private Animator cartAnimator;
    
    [Header("Cart Controls")]
    [SerializeField] private float cartMinSpeed = 1f;
    [SerializeField] private float cartMaxSpeed = 5f;
    [SerializeField] private float cartPumpAcceleration = 0.2f;
    [SerializeField] [Tooltip("In speed/s (speed is reduced by X every second)")] private float cartDeccelerationRate = 0.1f;

    [Header("Ramps Acceleration")] 
    [SerializeField] private AnimationCurve accelerationCurve;

    [Header("Others")] 
    [SerializeField] private float finishDistanceFromEnd = 5f;
    [SerializeField] private Quaternion rotationOffset = Quaternion.identity;
    private float distanceTravelled = 0f;

    [Header("Events")] 
    [SerializeField] private UnityEvent onCartLean;
    [SerializeField] private UnityEvent onCartLeanStop;

    public UnityEvent OnCartLean => onCartLean;
    public UnityEvent OnCartLeanStop => onCartLeanStop;

    [Header("Display info")]
    [SerializeField] private float cartSpeed = 0f;
    [SerializeField] private float currentRampDegree = 0f;

    public bool canMove = true;
    
    private float rampCoef = 1f;
    public float CurrentRampDegree => currentRampDegree;

    private int leanDirection = 0;
    public int LeanDirection => leanDirection;

    public float CartMaxSpeed => cartMaxSpeed;
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
        hitboxManager.onHit.AddListener(v => DeccelerateCart());
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

        if (canMove)
        {
            distanceTravelled += Time.deltaTime * cartSpeed;
        }

        transform.position = path.path.GetPointAtDistance(distanceTravelled);
        transform.rotation = path.path.GetRotationAtDistance(distanceTravelled) * rotationOffset;

        // Get direction at pos and determine angle
        Vector3 directionAtPos = path.path.GetDirectionAtDistance(distanceTravelled);
        float angleAtDistance = Vector3.Angle(-Vector3.up, directionAtPos);

        // offset angle so there's negative value for down ramp
        currentRampDegree = angleAtDistance - 90f;
        
        rampCoef = accelerationCurve.Evaluate(currentRampDegree);
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
        
        leanDirection = direction;
        
        if (direction == -1)
        {
            // Leaning to left, disable left hibox
            hitboxManager.SetActiveHitbox((int) HitboxManager.Hitbox.DownRight, false);
            onCartLean?.Invoke();
        }
        else if (direction == 1)
        {
            // Leaning to right, disable left hibox
            hitboxManager.SetActiveHitbox((int) HitboxManager.Hitbox.DownLeft, false);
            onCartLean?.Invoke();
        }
        else
        {
            onCartLeanStop?.Invoke();
        }
        
        // graphicsObject.transform.localEulerAngles = new Vector3(-40 * direction, 0,0);
    }

    private void HasReachedEnd()
    {
        float endLength = path.path.length - finishDistanceFromEnd;
        
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        float endLength = path.path.length - finishDistanceFromEnd;
        Gizmos.DrawCube(path.path.GetPointAtDistance(endLength), Vector3.one * 3f); 
    }
    #endif
}
