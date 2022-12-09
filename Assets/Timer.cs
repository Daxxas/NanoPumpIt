using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private GameManager gameState;

    // Update is called once per frame
    void Update()
    {
        TimeSpan time = TimeSpan.FromSeconds(gameState.GetTime());
        string displayTime = time.ToString();

        Text text = GetComponent<Text>();
        text.text = displayTime;
    }
}
