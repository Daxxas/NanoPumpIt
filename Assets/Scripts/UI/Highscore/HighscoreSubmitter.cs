using System.Collections.Generic;
using UnityEngine;


public class HighscoreSubmitter : MonoBehaviour
{
    [SerializeField] private List<LetterSelector> letters = new List<LetterSelector>();
    [SerializeField] private Timer timer;
    [SerializeField] private HighscoreManager highscoreManager;
    
    public void SubmitScore()
    {
        string name = "";
        foreach (var letter in letters)
        {
            name += letter.CurrentLetter;
        }

        float time = timer.DefaultTime - timer.GetTime();

        highscoreManager.RegisterNewHighscore(time, name);
    }
}