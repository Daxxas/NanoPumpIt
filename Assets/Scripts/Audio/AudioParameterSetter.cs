using System;
using FMODUnity;
using UnityEngine;


public class AudioParameterSetter : MonoBehaviour
{
    [ParamRef] [SerializeField] private string parameter;
    [SerializeField] private float value;
    
    public void SetParameter()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName(parameter,value);
    }
}