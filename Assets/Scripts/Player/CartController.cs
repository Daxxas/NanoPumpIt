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
    [SerializeField] private float cartMinSpeed = 0f;
    [SerializeField] private float cartPumpAcceleration = 0.2f;
    [Tooltip("In speed/s (speed is reduced by X every second)")]
    [SerializeField] private float cartDeccelerationRate = 0.1f;

    [Header("Ramps Acceleration")]
    [SerializeField] private AnimationCurve accelerationCurve;

    [Header("Others")]
    [SerializeField] private float finishDistanceFromEnd = 5f;
    [SerializeField] private Quaternion rotationOffset = Quaternion.identity;
    private float distanceTravelled = 0f;
    public float DistanceTravelled => distanceTravelled;

    [Header("Events")]
    [SerializeField] private UnityEvent onCartLean;
    [SerializeField] private UnityEvent onCartLeanStop;
    [SerializeField] private UnityEvent onCartReachEnd;

    public UnityEvent OnCartLean => onCartLean;
    public UnityEvent OnCartLeanStop => onCartLeanStop;

    public UnityEvent OnCartReachEnd => onCartReachEnd;

    [Header("Display info")]
    [SerializeField] private float cartSpeed = 0f;
    [SerializeField] private float currentRampDegree = 0f;
    
    public bool canMove = true;
    
    private bool hasCartReachedEnd = false;
    public bool HasCartReachedEnd => hasCartReachedEnd;

    private float rampCoef = 1f;
    public float CurrentRampDegree => currentRampDegree;

    private int leanDirection = 0;
    public int LeanDirection => leanDirection;

    public float CartSpeed
    {
        get => cartSpeed;
        set
        {
            cartSpeed = Mathf.Max(value,cartMinSpeed);
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

    // Update is called once per frame
    void Update()
    {
        if (playerInputsHolder.InputProviders[0] != null && playerInputsHolder.InputProviders[1] != null)
        {
            ApplyAcceleration();
            ApplySpeed();  
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
            charactersAnimators[0].SetFloat("PUSH_PULL", -playerInputsHolder.InputProviders[0].getLeanValue());
            charactersAnimators[1].SetFloat("PUSH_PULL", playerInputsHolder.InputProviders[1].getLeanValue());
        }

    }

    private void FixedUpdate()
    {
        UpdateRampCoef();
    }

    private void UpdateRampCoef()
    {
        // Get direction at pos and determine angle
        Vector3 directionAtPos = path.path.GetDirectionAtDistance(distanceTravelled);
        float angleAtDistance = Vector3.Angle(-Vector3.up, directionAtPos);

        // offset angle so there's negative value for down ramp
        currentRampDegree = angleAtDistance - 90f;

        rampCoef = accelerationCurve.Evaluate(currentRampDegree);
        // Debug.Log(rampDegree + " " + rampCoef);
    }

    private void ApplyAcceleration()
    {
        float oldCartSpeed = CartSpeed;

        CartSpeed -= cartDeccelerationRate * Time.deltaTime * oldCartSpeed;

        if (oldCartSpeed < 0)
        {
            CartSpeed += 5f * Time.deltaTime;
        }
         
        //CartSpeed += rampCoef * Time.fixedDeltaTime;
    }

    private void ApplySpeed()
    {
        if (canMove)
        {
            distanceTravelled += Time.deltaTime * cartSpeed;
            
            if(distanceTravelled >= path.path.length - finishDistanceFromEnd && !hasCartReachedEnd)
            {
                hasCartReachedEnd = true;
                OnCartReachEnd.Invoke();
            }
            
            // Prevent cart from going too far
            if (distanceTravelled >= path.path.length)
            {
                distanceTravelled = path.path.length-0.01f; // -0.01f to prevent cart from going back to the beginning
                canMove = false;
            }
        }

        transform.position = path.path.GetPointAtDistance(distanceTravelled);
        transform.rotation = path.path.GetRotationAtDistance(distanceTravelled) * rotationOffset;
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
        if (distanceTravelled > endLength)
        {
            GameManager.Instance.StopGame(GameManager.EndCondition.Win);
        }
        
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (path != null)
        {
            float endLength = path.path.length - finishDistanceFromEnd;
            Gizmos.DrawCube(path.path.GetPointAtDistance(endLength), Vector3.one * 1f);
        }
    }
    #endif
}
