using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayersController : MonoBehaviour
{
    [SerializeField] private HitboxManager hitboxManager;
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private CartController cartController;  
    private InputProvider[] inputProviders = new InputProvider[2];
    
    private int playerIndexTurn = 0;

    public InputProvider[] InputProviders => inputProviders;

    public void Update()
    {
        if ((inputProviders[0] != null) && (inputProviders[1] != null))
        {
            int leanDir0 = inputProviders[0].getLeanDirection();
            int leanDir1 = inputProviders[1].getLeanDirection();
            if ((leanDir0 == leanDir1) && (leanDir0 != 0))
            {
                cartController.Lean(leanDir0);
            }
            else
            {
                cartController.Lean(0);
            }
        }
        
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
            Debug.Log("Wrong pump !");
        }
    
    }

    private void SwitchPlayerIndex()
    {
        // alternate between 0 and 1
        playerIndexTurn = (playerIndexTurn + 1) % 2;
        if (playerIndexTurn == 0)
        {
            hitboxManager.SetActiveHitbox((int) HitboxManager.Hitbox.Left, false);
            hitboxManager.SetActiveHitbox((int) HitboxManager.Hitbox.Right, true);
        }
        else
        {
            hitboxManager.SetActiveHitbox((int) HitboxManager.Hitbox.Left, true);
            hitboxManager.SetActiveHitbox((int) HitboxManager.Hitbox.Right, false);
        }
    }
}
