using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class CartController : MonoBehaviour
{
    [SerializeField] private PathCreator path;
    
    [SerializeField] private float cartSpeed = 0f;
    [SerializeField] private Quaternion rotationOffset = Quaternion.identity;
    [SerializeField] private Vector3 positionOffset;
    private float distanceTravelled = 0f;
    

    // Update is called once per frame
    void Update()
    {
        distanceTravelled += Time.deltaTime * cartSpeed;
        transform.position = path.path.GetPointAtDistance(distanceTravelled) + positionOffset;
        transform.rotation = path.path.GetRotationAtDistance(distanceTravelled) * rotationOffset;
    }
}
