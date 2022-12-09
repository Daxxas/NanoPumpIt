using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayersController : MonoBehaviour
{
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private CartController cartController;  
    private InputProvider[] inputProviders = new InputProvider[2];
    
    private int playerIndexTurn = 0;

    public InputProvider[] InputProviders => inputProviders;

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
            Debug.Log("Wrong pump !");
        }
    
    }
    
    private void SwitchPlayerIndex()
    {
        // alternate between 0 and 1
        playerIndexTurn = (playerIndexTurn + 1) % 2;
    }
}
