//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 27 Jan 2024

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class NumberHolder : MonoBehaviour
{
    public float value;    

    [Header("Display Parameters")]
    public string prefix = "$ ";
    public string suffix = " remaining";
    public string format = "N2";
    public TextMeshPro textDisplay;
    public TextMeshProUGUI textDisplayUGui;

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
        value -= delta;
    }

    public void Multiply(float factor)
    {
        value *= factor;
    }

    public void Divide(float denominator)
    {
        value /= denominator;
    }

    public void SetValue(float newValue)
    {
        value = newValue;
    }

    public void SetRandomDelta(float range)
    {
        float delta = Random.Range(-range, range);
        Add(delta);
    }

    public void AddRandom(float range)
    {
        float delta = Random.Range(0, range);
        Add(delta);
    }

    public void SubtractRandom(float range)
    {
        float delta = Random.Range(0, range);
        Add(-delta);
    }

    public void MultiplyRandomPositive(float range)
    {
        float delta = Random.Range(0, range);
        Multiply(delta);
    }

    public void MultiplyRandomNegative(float range)
    {
        float delta = Random.Range(0, range);
        Multiply(-delta);
    }

    public void MultiplyRandomRange(float range)
    {
        float delta = Random.Range(-range, range);
        Multiply(delta);
    }

    public void DivideRandomPositive(float range)
    {
        float delta = Random.Range(0, range);
        Divide(delta);
    }

    public void DivideRandomNegative(float range)
    {
        float delta = Random.Range(0, range);
        Divide(-delta);
    }

    public void DivideRandomRange(float range)
    {
        float delta = Random.Range(-range, range);
        Divide(delta);
    }

    public int GetIntValue()
    {
        return (int)value;
    }
}
