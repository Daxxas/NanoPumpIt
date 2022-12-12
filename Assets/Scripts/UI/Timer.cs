using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI text;

    private float defaultTime = 180f;
    private float time;
    private bool playing = false;

    private void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            time -= Mathf.Max(Time.deltaTime, 0);
            UpdateText();
        }
        
    }

    void UpdateText()
    {
        TimeSpan timespan = TimeSpan.FromSeconds(time);
        string displayTime = string.Format("{0:00}:{1:00}.{2:000}", timespan.Minutes, timespan.Seconds, timespan.Milliseconds);
        text.text = displayTime;
    }

    public float GetTime()
    { return time; }

    public void Play()
    { playing = true; }
    public void Pause()
    { playing = false; }

    public void Reset()
    {
        time = defaultTime;
        UpdateText();
    }

}
