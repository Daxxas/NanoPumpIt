using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputProvider : MonoBehaviour
{
    public Action onPump;
    public Action<Vector2> onDodge;

    public void Pump(InputAction.CallbackContext context)
    {
        onPump?.Invoke();
    }

    public void Dodge(InputAction.CallbackContext context)
    {
        onDodge?.Invoke(context.ReadValue<Vector2>());
    }
    
}