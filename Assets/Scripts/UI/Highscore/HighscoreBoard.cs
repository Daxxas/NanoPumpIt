using System;
using MoreMountains.Feedbacks;
using UnityEngine;


public class HighscoreBoard : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HighscoreManager highscoreManager;
    [SerializeField] private GameObject highscoreEntryPrefab;
    [SerializeField] private Transform highscoreEntryContainer;
    
    [Header("Feedbacks")]
    [SerializeField] private MMF_Player showFeedback;
    [SerializeField] private MMF_Player hideFeedback;

    [Header("Settings")] 
    [SerializeField] private Color firstColor;
    [SerializeField] private Color lastColor;
    
    
    private void Start()
    {
        RefreshBoard();
    }

    [ContextMenu("Refresh Board")]
    private void RefreshBoard()
    {
        ClearBoard();
     
        highscoreManager.ReloadHighscores();

        for (int i = 0; i < highscoreManager.Highscores.Count; i++)
        {
            var highscoreEntry = highscoreManager.Highscores[i];
            
            var highscoreEntryGameObject = Instantiate(highscoreEntryPrefab, highscoreEntryContainer);
            var highscoreEntryUI = highscoreEntryGameObject.GetComponent<HighscoreEntryUI>();
            Color color = Color.Lerp(firstColor, lastColor, (float)i / highscoreManager.Highscores.Count);
            highscoreEntryUI.SetHighscoreEntry(i, color, highscoreEntry);
        }
    }

    private void ClearBoard()
    {
        for (int i = 0; i < highscoreEntryContainer.childCount; i++)
        {
            Destroy(highscoreEntryContainer.GetChild(i).gameObject);
        }
    }

    [ContextMenu("Hide")]
    public void HideBoard()
    {
        hideFeedback.PlayFeedbacks();
    }

    [ContextMenu("Show")]
    public void ShowBoard()
    {
        showFeedback.PlayFeedbacks();
    }
}