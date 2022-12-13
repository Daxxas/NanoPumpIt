using System;
using MoreMountains.Feedbacks;
using UnityEngine;

public class TransitionUI : MonoBehaviour
{
    [SerializeField] private MMF_Player hideTransition;
    [SerializeField] private MMF_Player displayTransition;

    public void Transition(bool on, Action onComplete)
    {
        if (!on)
        {
            hideTransition.PlayFeedbacks();
            hideTransition.Events.OnComplete.AddListener(() =>
            {
                onComplete?.Invoke();
                hideTransition.Events.OnComplete.RemoveAllListeners();
            });
        }
        else
        {
            displayTransition.PlayFeedbacks();
            displayTransition.Events.OnComplete.AddListener(() =>
            {
                onComplete?.Invoke();
                displayTransition.Events.OnComplete.RemoveAllListeners();
            });

        }
    }
}