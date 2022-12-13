using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class GlobalVolumeController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Volume volume;
    [SerializeField] private CartController cartController;

    /*
State 1 - normal (tout le temps)

Vignette : Intensity 0.43 / color : 2B0B1F
Color Adjustements : color filter : tout blanc
Lens distortion : 0
Chromatic aberration : 0.02

State 2 - Acceleration (dure le temps de l'accéleration)

Vignette : Intensity 0.5 / color : 180B2B
Color Adjustements : color filter : tout blanc
Lens distortion : -0.17
Chromatic aberration : 0.2
 */

    [Header("Normal state")]
    [SerializeField] private float normalVignetteIntensity;
    [SerializeField] private Color normalVignetteColor;
    [SerializeField] private Color normalColorAdjustementsFilter;
    [SerializeField] private float normalLensDistortionIntensity;
    [SerializeField] private float normalChromaticAberrationIntensity;

    [Header("Speed state")] 
    [SerializeField] private float speedVignetteIntensity;
    [SerializeField] private Color speedVignetteColor;
    [SerializeField] private Color speedColorAdjustementsFilter;
    [SerializeField] private float speedLensDistortionIntensity;
    [SerializeField] private float speedChromaticAberrationIntensity;

    private Vignette vignette;
    private ColorAdjustments colorAdjustments;
    private LensDistortion lensDistortion;
    private ChromaticAberration chromaticAberration;

    private float currentStyle = 0f;

    private void Start()
    {
       volume.profile.TryGet<Vignette>(out vignette);
       volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);
       volume.profile.TryGet<LensDistortion>(out lensDistortion);
       volume.profile.TryGet<ChromaticAberration>(out chromaticAberration);
    }
    
    private void Update()
    {
        float targetStyle = Sigmoid(Mathf.Log(Mathf.Max(cartController.CartSpeed, 0.01f) * 0.1f));
        currentStyle = Mathf.Lerp(currentStyle, targetStyle, Time.deltaTime);
        SetStyle(currentStyle);
        //chromaticAberration.intensity.value = 200f;
    }
     
    private void SetStyle(float speedVal)
    {
        vignette.intensity.value = Mathf.Lerp(normalVignetteIntensity, speedVignetteIntensity, speedVal);
        vignette.color.value = Color.Lerp(normalVignetteColor, speedVignetteColor, speedVal);
        colorAdjustments.colorFilter.value = Color.Lerp(normalColorAdjustementsFilter, speedColorAdjustementsFilter, speedVal);
        lensDistortion.intensity.value = Mathf.Lerp(normalLensDistortionIntensity, speedLensDistortionIntensity, speedVal);
        chromaticAberration.intensity.value = Mathf.Lerp(normalChromaticAberrationIntensity, speedChromaticAberrationIntensity, speedVal);
    }

    public static float Sigmoid(float value)
    {
        return (float)(1.0 / (1.0 + Math.Pow(Math.E, -value)));
    }
}