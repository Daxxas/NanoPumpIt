using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayersController : MonoBehaviour
{
    private InputProvider[] inputProviders = new InputProvider[2];
    private int playerIndex = 0;

    public InputProvider[] InputProviders
    {
        get;
    }

    public void AddPlayer(PlayerInput playerInput)
    {
        inputProviders[playerIndex] = playerInput.GetComponent<InputProvider>();

        inputProviders[playerIndex].onPump += Pump;
        playerIndex++;
    }

    public void Pump()
    {
        Debug.Log("test");
    }
}
