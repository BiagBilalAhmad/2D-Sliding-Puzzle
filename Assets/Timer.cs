using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;  // Reference to the TextMeshProUGUI component
    private float startTime;
    private bool isRunning = false;
    float timeElapsed = 0f;

    void Start()
    {
        StartTimer();
    }

    void Update()
    {
        if (isRunning)
        {
            timeElapsed = Time.time - startTime;
            
            timerText.text = GetTimeString();
        }
    }

    public float GetTime()
    {
        return timeElapsed;
    }

    public string GetTimeString()
    {
        int minutes = Mathf.FloorToInt(timeElapsed / 60F);
        int seconds = Mathf.FloorToInt(timeElapsed % 60F);
        string timeText = string.Format("{0:0}:{1:00}", minutes, seconds);
        return timeText;
    }

    public void StartTimer()
    {
        startTime = Time.time;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        startTime = Time.time;
    }
}
