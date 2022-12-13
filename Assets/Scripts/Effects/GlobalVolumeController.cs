using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class GlobalVolumeController : MonoBehaviour
{
    [SerializeField] private Volume volume;

    [Header("Normal state")]
    [SerializeField] private float normalVignetteIntensity;

    [Header("Speed state")] 
    [SerializeField] private float speedVignetteIntensity;
    
    private void Start()
    {
       volume.profile.TryGet<Vignette>(out var vignette);
       volume.profile.TryGet<ColorAdjustments>(out var colorAdjustments);
       volume.profile.TryGet<LensDistortion>(out var lensDistortion);
       volume.profile.TryGet<ChromaticAberration>(out var chromaticAberration);
    }

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
    
    private void Update()
    {
        
    }
}