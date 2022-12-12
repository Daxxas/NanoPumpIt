using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        instance = this;
        
        DontDestroyOnLoad(this.gameObject);
    }
    
    public enum GameState
    {
        Menu,
        Gameplay,
        End
    }

    [Header("References")] 
    [SerializeField] private PumpController pumpController;
    [SerializeField] private CartController cartController;
    [SerializeField] private HighscoreBoard highscoreBoard;
    [SerializeField] private PlayerInputsHolder playerInputsHolder;
    
    [SerializeField] private GameState gameState;

    private void Start()
    {
        pumpController.onPump.AddListener(StartGame);
    }

    private void StartGame()
    {
        // Game starts if both inputs provider exists & a first pump has been executed
        if (gameState == GameState.Menu && playerInputsHolder.InputProviders[0] != null && playerInputsHolder.InputProviders[1] != null)
        {
            gameState = GameState.Gameplay;
            cartController.canMove = true;
            highscoreBoard.HideBoard();
        }
    }

    public void StopGame()
    {
        // Logic when the game ends
        if (gameState != GameState.End)
        {
            gameState = GameState.End;
            
            
        }
    }

    public void ResetGame()
    {
        gameState = GameState.Menu;
        // Logic to restart the game
    }
    
    
    private float time = 60 * 3;
    
    public float GetTime()
    { return time; }
    
    // Update is called once per frame
    void Update()
    {
        time -= Mathf.Max(Time.deltaTime,0);
    }
}
