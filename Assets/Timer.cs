using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private GameManager gameState;

    
    
    // TODO : Je suis pas sûr de l'orga du code pour le timer 
    // TODO si tu vois ce commentaire et qu'on en a pas parlé c'est que j'ai oublié de venir t'en parler
    
    
    
    // Update is called once per frame
    void Update()
    {
        TimeSpan time = TimeSpan.FromSeconds(gameState.GetTime());
        string displayTime = time.ToString();

        Text text = GetComponent<Text>();
        text.text = displayTime;
    }
}
