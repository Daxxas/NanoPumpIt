using System;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Collider))]
public class PlayerHitbox : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleLayer;

    public Action<Vector3> onHit;
    public UnityEvent onHitUnity;

    private Collider boxCollider;

    public Collider Collider
    {
        get
        {
            if (boxCollider == null)
            {
                boxCollider = GetComponent<Collider>();
            }
            return boxCollider;
        }
    }

    private void Start()
    {
        Debug.Log("HITBOX : " + gameObject.name);
        boxCollider = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((obstacleLayer.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            var collisionPoint = boxCollider.ClosestPoint(transform.position);
            
            // Collision normal
            //var collisionNormal = transform.position - collisionPoint;

            Debug.Log("Obstacle hit !");
            onHit?.Invoke(collisionPoint);
            onHitUnity?.Invoke();
        }
    }
}