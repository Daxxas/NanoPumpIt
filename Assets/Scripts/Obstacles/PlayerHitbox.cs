using System;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class PlayerHitbox : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleLayer;

    public Action<Vector3> onHit;

    private Collider collider;

    public Collider Collider => collider;

    private void Awake()
    {
        collider = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((obstacleLayer.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            var collisionPoint = collider.ClosestPoint(transform.position);
            
            // Collision normal
            //var collisionNormal = transform.position - collisionPoint;

            Debug.Log("Obstacle hit !");
            onHit?.Invoke(collisionPoint);
        }
    }
}