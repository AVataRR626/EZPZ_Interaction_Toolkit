using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightModifier : MonoBehaviour
{
    public Light lightSource;
    public Color colourDelta;

    private void Start()
    {
        if (lightSource == null)
            lightSource = GetComponent<Light>();
    }

    public void AddColour()
    {
        lightSource.color += colourDelta;
    }

    public void SubtractColour()
    {
        lightSource.color -= colourDelta;
    }

    public void ColourAdd()
    {
        AddColour();
    }

    public void ColourSubtract()
    {
        SubtractColour();
    }
}
