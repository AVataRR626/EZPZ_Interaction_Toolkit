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
    public UnityEvent onClockZero;

    [Header("Display Parameters")]
    public TextMeshPro textDisplay;

    public bool triggerFlag;

    private void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isActiveAndEnabled)
        {
            if (clock > 0)
            {
                clock -= Time.fixedDeltaTime;

                if (clock < 0)
                    clock = 0;

                if (textDisplay != null)
                    textDisplay.text = clock.ToString("N3");
            }
            else
            {
                if (!triggerFlag)
                {
                    triggerFlag = true;
                    onClockZero.Invoke();
                }

                clock = 0;
            }
        }
        
    }

    public void Reset()
    {
        clock = startingTime;
        triggerFlag = false;
    }
}
