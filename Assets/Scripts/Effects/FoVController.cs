using System;
using Cinemachine;
using UnityEngine;


public class FoVController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CartController cartController;
    [SerializeField] private CinemachineVirtualCamera camera;

    [Header("Settings")]
    [SerializeField] private float minFOV = 60f;
    [SerializeField] private float maxFOV = 90f;
    [SerializeField] private float speedForMaxFOV = 5f;
    
    private float currentFOV;

    private void Start()
    {
        currentFOV = minFOV;
    }

    private void Update()
    {
        float fovDelta = maxFOV - minFOV;
        float targetFOV = minFOV + fovDelta * cartController.CartSpeed / speedForMaxFOV;
        currentFOV = Mathf.Lerp(currentFOV, targetFOV, Time.deltaTime);
        currentFOV = Mathf.Clamp(currentFOV, minFOV, maxFOV);
        camera.m_Lens.FieldOfView = currentFOV;
    }
}