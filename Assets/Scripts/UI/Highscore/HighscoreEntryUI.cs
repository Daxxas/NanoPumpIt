using System;
using TMPro;
using UnityEngine;

public class HighscoreEntryUI : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private TextMeshProUGUI TMPText;

    public void SetHighscoreEntry(int position, Color color, HighscoreManager.Highscore highscore)
    {
        position++;
        TMPText.text = position + ". " + highscore.name + " - " + FloatToTime(highscore.time);
        TMPText.color = color;
    }

    private string FloatToTime(float time)
    {
        // Convert float time to display string
        var ts = TimeSpan.FromSeconds(time);
        string text =string.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds);

        return text;
    }
}