using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayersController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HitboxManager hitboxManager;
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private CartController cartController;
    
    [Header("Animators")]
    [SerializeField] private Animator[] charactersAnimators;
    [SerializeField] private Animator cartAnimator;
    
    private InputProvider[] inputProviders = new InputProvider[2];
    
    private int playerIndexTurn = 0;
    private float pumpEquilibrium = 1f;
    public InputProvider[] InputProviders => inputProviders;

    public void Update()
    {
        if ((inputProviders[0] != null) && (inputProviders[1] != null))
        {
            // int lean inputs
            int leanDir0 = inputProviders[0].getLeanDirection();
            int leanDir1 = inputProviders[1].getLeanDirection();
            
            // Debug.Log("LeanValue P1 : " + leanDir0 + " LeanValue P2 : " + leanDir1);

            
            if ((leanDir0 == leanDir1) && (leanDir0 != 0))
            {
                cartController.Lean(leanDir0);
            }
            else
            {
                cartController.Lean(0);
            }

            cartAnimator.SetInteger("LeanDirection", cartController.LeanDirection);

            
            // lean animation
            charactersAnimators[0].SetFloat("PUSH_PULL", inputProviders[0].getLeanValue());
            charactersAnimators[1].SetFloat("PUSH_PULL", inputProviders[1].getLeanValue());
        }

        // HIGH LOW animation
        // Debug.Log(playerIndexTurn);
        // TODO : Demander à Lucas pourquoi y'a un *4 ici
        if (playerIndexTurn == 1) pumpEquilibrium += Time.deltaTime * 4;
        else if (playerIndexTurn == 0) pumpEquilibrium -= Time.deltaTime * 4;
        pumpEquilibrium = Mathf.Clamp01(pumpEquilibrium);

        charactersAnimators[0].SetFloat("HIGH_LOW", 1-pumpEquilibrium);
        charactersAnimators[1].SetFloat("HIGH_LOW", pumpEquilibrium);
    }
    public void Pump(int playerIndex)
    {
        if (playerIndex == playerIndexTurn)
        {
            SwitchPlayerIndex();
            cartController.AccelerateCart();
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
        cartAnimator.SetInteger("playerIndexTurn", playerIndexTurn);

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
