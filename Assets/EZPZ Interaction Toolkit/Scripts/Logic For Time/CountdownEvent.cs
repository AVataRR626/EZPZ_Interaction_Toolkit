//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 25 Jul 2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class CountdownEvent : MonoBehaviour
{
    [Header("Clock Parameters")]
    public float startingTime;
    public float clock;
    public bool looping = false;
    public UnityEvent onClockZero;
    public UnityEvent onReset;

    [Header("Display Parameters")]
    public TextMeshPro textDisplay;
    public TextMeshProUGUI textDisplayUGui;

    public bool triggerFlag;
    public bool clockRunning = true;

    public void OnEnable()
    {
        Reset();
    }

    private void Start()
    {
        //Reset();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isActiveAndEnabled)
        {
            if (clockRunning)
            {
                if (clock > 0)
                {
                    clock -= Time.fixedDeltaTime;

                    if (clock < 0)
                        clock = 0;

                    if (textDisplay != null)
                        textDisplay.text = clock.ToString("N3");

                    if (textDisplayUGui != null)
                        textDisplayUGui.text = clock.ToString("N3");
                }
                else
                {
                    if (!triggerFlag)
                    {
                        triggerFlag = true;
                        onClockZero.Invoke();
                    }

                    clock = 0;

                    if (looping)
                    {
                        Reset();
                    }
                }
            }
        }
        
    }

    public void Reset()
    {
        clock = startingTime;
        triggerFlag = false;

        onReset.Invoke();
    }

    public void PauseClock()
    {
        clockRunning = false;
    }

    public void ResumeClock()
    {
        StartClock();
    }

    public void UnpauseClock()
    {
        StartClock();
    }

    public void StartClock()
    {
        clockRunning = true;
    }
}
