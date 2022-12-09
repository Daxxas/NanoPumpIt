using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayersController : MonoBehaviour
{
    [SerializeField] private CartController cartController;  
    private InputProvider[] inputProviders = new InputProvider[2];
    private int playerCount = 0;
    
    private int playerIndexTurn = 0;

    public InputProvider[] InputProviders
    {
        get;
    }

    public void AddPlayer(PlayerInput playerInput)
    {
        inputProviders[playerCount] = playerInput.GetComponent<InputProvider>();
        playerInput.GetComponent<PlayerInfo>().PlayerIndex = playerCount;

        inputProviders[playerCount].onPump += Pump;
        playerCount++;
    }

    public void Pump(int playerIndex)
    {
        if (playerIndex == playerIndexTurn)
        {
            SwitchPlayerIndex();
            cartController.AccelerateCart();
            Debug.Log("test " + playerIndex);
        }
        else
        {
            // Player who pressed pump button is not the one who should pump
        }
    
    }
    
    private void SwitchPlayerIndex()
    {
        // alternate between 0 and 1
        playerIndexTurn = (playerIndexTurn + 1) % playerCount;
    }
}
