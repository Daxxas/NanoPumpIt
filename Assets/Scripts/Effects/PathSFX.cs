using System;
using PathCreation;
using UnityEngine;
using UnityEngine.Events;

public class PathSFX : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CartController cartController;
    [SerializeField] private PathCreator path;

    [Header("Settings")] 
    [SerializeField] private float distanceFromPathStart;
    [SerializeField] private UnityEvent onPathStart;
    [SerializeField] private float middlePointOfPath;
    [SerializeField] private UnityEvent onPathMiddle;

    private bool passedStart = false;
    private bool passedMiddle = false;
    
    private void Update()
    {
        if (cartController.DistanceTravelled >= distanceFromPathStart && !passedStart)
        {
            passedStart = true;
            onPathStart.Invoke();
        }
        if (cartController.DistanceTravelled >= middlePointOfPath && !passedMiddle)
        {
            passedMiddle = true;
            onPathMiddle.Invoke();
        }
    }
    
    #if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (path != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawCube(path.path.GetPointAtDistance(distanceFromPathStart), Vector3.one * 1f);
            Gizmos.color = Color.green;
            Gizmos.DrawCube(path.path.GetPointAtDistance(middlePointOfPath), Vector3.one * 1f);
        }
    }
#endif
}