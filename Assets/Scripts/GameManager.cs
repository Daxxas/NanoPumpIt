using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public enum GameState
    {
        menu,
        gameplay,
        end
    }

    [SerializeField] private GameState gameState;

    private float time = 60 * 3;

    public float GetTime()
    { return time; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time -= Mathf.Max(Time.deltaTime,0);
    }
}
