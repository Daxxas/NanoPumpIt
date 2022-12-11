using System;
using TMPro;
using UnityEngine;

public class HighscoreEntryUI : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private TextMeshProUGUI TMPText;

    public void SetHighscoreEntry(int position, HighscoreManager.Highscore highscore)
    {
        position++;
        TMPText.text = position + ". " + highscore.name + " - " + FloatToTime(highscore.time);
    }

    private string FloatToTime(float time)
    {
        // Convert float time to display string
        var ts = TimeSpan.FromSeconds(time);
        string text =string.Format("{0:00}:{1:00},{2:000}", ts.Minutes, ts.Seconds, ts.Milliseconds);

        return text;
    }
}