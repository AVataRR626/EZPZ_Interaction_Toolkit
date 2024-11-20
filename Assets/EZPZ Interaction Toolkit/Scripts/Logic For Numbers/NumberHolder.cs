//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 27 Jan 2024

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UIElements;
using JetBrains.Annotations;

public class NumberHolder : MonoBehaviour
{
    public float value;    

    [Header("Display Parameters")]
    public string prefix = "$ ";
    public string suffix = " remaining";
    public string format = "N2";
    public TextMeshPro textDisplay;
    public TextMeshProUGUI textDisplayUGui;

    [Header("Peer Number Holders (for comparisons)")]
    public List<NumberHolder> numberHolderPeers;
    public UnityEvent onGreatest;

    public void Update()
    {
        if (textDisplay != null)
            textDisplay.text = prefix + value.ToString(format) + suffix;

        if (textDisplayUGui != null)
            textDisplayUGui.text = prefix + value.ToString(format) + suffix;
    }

    public void Add(float delta)
    {
        value += delta;
    }

    public void Subtract(float delta)
    {
        value -= 1;
    }

    public void SetValue(float newValue)
    {
        value = newValue;
    }

    public void CheckIfGreatest()
    {
        float greatest = value;

        for(int i = 0; i < numberHolderPeers.Count; i++)
        {
            if(greatest < numberHolderPeers[i].value)
            {
                greatest = numberHolderPeers[i].value;
            }
        }

        if(value == greatest)
        {
            onGreatest.Invoke();
        }
    }
}
