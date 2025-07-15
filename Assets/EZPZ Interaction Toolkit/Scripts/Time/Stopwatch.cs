using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Stopwatch : MonoBehaviour
{
    public float clock;
    public float stopTime;
    public UnityEvent onPause;
    public UnityEvent onStart;

    [Header("Display Parameters")]
    public TextMeshPro textDisplay;
    public TextMeshProUGUI textDisplayUGui;
    public string format = "N2";

    [Header("System Stuff - Usually Don't Touch")]
    public bool stopTimeFlag = false;
    public bool clockRunning = false;

    // Update is called once per frame
    void Update()
    {
        if (clockRunning)
        {
            clock += Time.deltaTime;

            HandleStopTime();
        }

        if (textDisplay != null)
            textDisplay.text = GenUtils.HMSFormat(clock, format);

        if (textDisplayUGui != null)
            textDisplayUGui.text = GenUtils.HMSFormat(clock, format);

    }

    public void HandleStopTime()
    {
        if (stopTime > 0)
        {            
            if(clock >= stopTime)
            {
                clock = stopTime;

                if (!stopTimeFlag)
                {   
                    PauseClock();
                    stopTimeFlag = true;
                }
            }
        }
    }

    public void StartClock()
    {
        clockRunning = true;
        stopTimeFlag = false;
        onStart.Invoke();
    }

    public void PauseClock()
    {
        clockRunning = false;
        onPause.Invoke();
    }

    public void UnpauseClock()
    {
        StartClock();
    }

    public void RestartClock()
    {
        ResetClock();
        UnpauseClock();
    }

    public void ResetClock()    
    {
        clock = 0;
        clockRunning = false;
        stopTimeFlag = false;
    }

    public void ResumeClock()
    {
        StartClock();
    }
}
