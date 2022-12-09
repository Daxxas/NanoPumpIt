using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInputManagerController : MonoBehaviour
{
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private PlayersController playersController;

    private int playerCount = 0;
    
    public void AddPlayer(PlayerInput playerInput)
    {
        playersController.InputProviders[playerCount] = playerInput.GetComponent<InputProvider>();
        playerInput.GetComponent<PlayerInfo>().PlayerIndex = playerCount;

        playersController.InputProviders[playerCount].onPump += playersController.Pump;
        //playersController.InputProviders[playerCount].onLean += playersController.Lean;
        playerCount++;
        TryDisableJoin();
    }
    
    public void TryDisableJoin()
    {
        if (playerInputManager.playerCount >= 2)
        {
            playerInputManager.DisableJoining();
        }
    }
}