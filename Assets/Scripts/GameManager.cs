using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        instance = this;
        
        //DontDestroyOnLoad(this.gameObject);
    }
    
    public enum GameState
    {
        Menu,
        Gameplay,
        End
    }

    public enum EndCondition
    {
        Win,
        Lose
    }

    [Header("References")] 
    [SerializeField] private PumpController pumpController;
    [SerializeField] private CartController cartController;
    [SerializeField] private HighscoreBoard highscoreBoard;
    [SerializeField] private EndingUI endingUI;
    [SerializeField] private PlayerInputsHolder playerInputsHolder;
    [SerializeField] private Timer timer;

    [Header("Events")] 
    [SerializeField] private UnityEvent onWin;
    [SerializeField] private UnityEvent onLose;

    [SerializeField] private GameState gameState;

    
    
    private void Start()
    {
        pumpController.onPump.AddListener(StartGame);
        cartController.OnCartReachEnd.AddListener( () => StopGame(EndCondition.Win));
    }

    private void StartGame()
    {
        // Game starts if both inputs provider exists & a first pump has been executed
        if (gameState == GameState.Menu && playerInputsHolder.InputProviders[0] != null && playerInputsHolder.InputProviders[1] != null)
        {
            gameState = GameState.Gameplay;

            cartController.canMove = true;
            highscoreBoard.HideBoard();
            timer.Play();
        }
    }

    public void StopGame(EndCondition endCondition)
    {
        Debug.Log("Stop game !");
        // Logic when the game ends
        if (gameState != GameState.End)
        {
            gameState = GameState.End;
            timer.Pause();
            playerInputsHolder.InputProviders[0].onPump += i => ResetGame();
            playerInputsHolder.InputProviders[1].onPump += i => ResetGame();
            
            if (endCondition == EndCondition.Lose)
            {
                onLose?.Invoke();
                cartController.canMove = false;
                endingUI.DisplayLose();
            }
            else if (endCondition == EndCondition.Win)
            {
                // Save time
                onWin?.Invoke();
                endingUI.DisplayWin();
            }
        }
    }

    public void ResetGame()
    {
        Debug.Log("Reset Game");
        if (gameState == GameState.End)
        {
            playerInputsHolder.InputProviders[0].onPump -=  i => ResetGame();
            playerInputsHolder.InputProviders[1].onPump -=  i => ResetGame();
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
    }

    [ContextMenu("reload")]
    void DebugReload()
    {
        SceneManager.LoadScene("LucasScene");
    }

}
