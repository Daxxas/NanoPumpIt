using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class HighscoreSubmitter : MonoBehaviour
{
    [SerializeField] private List<LetterSelector> letters = new List<LetterSelector>();
    [SerializeField] private Timer timer;
    [SerializeField] private HighscoreManager highscoreManager;

    [SerializeField] private TextMeshProUGUI timeTMP;

    public void Display()
    {
        UpdateTime();
        gameObject.SetActive(true);
    }
    
    public void SubmitScore()
    {
        string name = "";
        foreach (var letter in letters)
        {
            name += letter.CurrentLetter.ToUpper();
        }

        float time = timer.DefaultTime - timer.GetTime();

        highscoreManager.RegisterNewHighscore(time, name);
    }

    public void UpdateTime()
    {
        float time = timer.DefaultTime - timer.GetTime();

        var ts = TimeSpan.FromSeconds(time);
        string text = string.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds);

        timeTMP.text = text;
    }
}