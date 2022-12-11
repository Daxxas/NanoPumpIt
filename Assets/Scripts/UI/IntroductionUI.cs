using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;


public class IntroductionUI : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private PumpController pumpController;
    [SerializeField] private HighscoreBoard highscoreBoard;
    [SerializeField] private Image[] pumpIndicators;
    [SerializeField] private Sprite pumpOn;
    [SerializeField] private Sprite pumpOff;
    [SerializeField] private MMF_Player fadeOut;
 
    [Header("Parameters")] 
    [SerializeField] private int pumpCountBeforeFadeOut = 6;

    private int pumpCount = 0;
    
    private void Start()
    {
        pumpController.OnPump.AddListener(CheckFadeOut);
    }

    private void Update()
    {
        for (int i = 0; i < pumpIndicators.Length; i++)
        {
            if (pumpController.CanPump)
            {
                pumpIndicators[i].sprite = pumpController.PlayerIndexTurn == i ? pumpOff : pumpOn;
            }
            else
            {
                pumpIndicators[i].sprite = pumpOff;
            }
        }
    }

    private void CheckFadeOut()
    {
        pumpCount++;
        if (pumpCount == 1)
        {
            highscoreBoard.HideBoard();
        }
        
        if (pumpCount >= pumpCountBeforeFadeOut)
        {
            fadeOut.PlayFeedbacks();
        }
    }
}