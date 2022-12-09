using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputProvider : MonoBehaviour
{
    private PlayerInfo playerInfo;

    public Action<int> onPump;

    private float leanValue = 0f;
    private int leanDirection = 0;

    private void Start()
    {
        playerInfo = GetComponent<PlayerInfo>();
    }

    public void Pump(InputAction.CallbackContext context)
    {
        if(context.performed)
            onPump?.Invoke(playerInfo.playerIndex);
    }

    public void Lean(InputAction.CallbackContext context)
    {
        leanValue = context.ReadValue<Vector2>().x;
        leanDirection = Mathf.RoundToInt(leanValue);
    }

    public float getLeanValue()
    { return leanValue; }
    public int getLeanDirection()
    { return leanDirection; }

}