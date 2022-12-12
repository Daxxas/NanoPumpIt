using System;
using FMODUnity;
using UnityEngine;


public class AudioParameterSetter : MonoBehaviour
{
    [ParamRef] [SerializeField] private string parameter;
    [SerializeField] private float value;
    
    public void ApplyParameter()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName(parameter,value);
    }

    public void ApplyParameter(float inputValue)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName(parameter,inputValue);
    }
}