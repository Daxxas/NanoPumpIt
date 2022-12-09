using System;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class PlayerHitbox : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleLayer;

    public Action onHit;

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
            Debug.Log("Obstacle hit !");
            onHit?.Invoke();
        }
    }
}