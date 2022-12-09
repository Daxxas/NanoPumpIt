using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputProvider : MonoBehaviour
{
    private PlayerInfo playerInfo;

    public Action<int> onPump;
    public Action<Vector2> onDodge;

    private void Start()
    {
        playerInfo = GetComponent<PlayerInfo>();
    }

    public void Pump(InputAction.CallbackContext context)
    {
        onPump?.Invoke(playerInfo.playerIndex);
    }

    public void Dodge(InputAction.CallbackContext context)
    {
        onDodge?.Invoke(context.ReadValue<Vector2>());
    }
    
}