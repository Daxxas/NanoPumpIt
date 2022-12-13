using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInputsHolder : MonoBehaviour
{
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private PumpController pumpController;

    private InputProvider[] inputProviders = new InputProvider[2];
    public InputProvider[] InputProviders => inputProviders;

    private int playerCount = 0;

    public Action onPlayerJoined;
    public Action onPlayerLeft;

    private void Start()
    {
        //DontDestroyOnLoad(gameObject);
    }

    public void AddPlayer(PlayerInput playerInput)
    {
        inputProviders[playerCount] = playerInput.GetComponent<InputProvider>();
        playerInput.GetComponent<PlayerInfo>().PlayerIndex = playerCount;

        inputProviders[playerCount].onPump += pumpController.Pump;
        //playersController.InputProviders[playerCount].onLean += playersController.Lean;
        playerCount++;
        TryDisableJoin();
        onPlayerJoined?.Invoke();
    }
    
    public void RemovePlayer()
    {
        onPlayerLeft?.Invoke();
    }

    public void TryDisableJoin()
    {
        if (playerInputManager.playerCount >= 2)
        {
            playerInputManager.DisableJoining();
        }
    }
}