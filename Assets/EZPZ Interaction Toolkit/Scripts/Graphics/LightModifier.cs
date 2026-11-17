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

    public void AddRed(float c)
    {
        Color newDelta = new Color(c,0,0);

        lightSource.color += newDelta;
    }

    public void SubtractRed(float c)
    {
        Color newDelta = new Color(c, 0, 0);

        lightSource.color -= newDelta;
    }

    public void AddGreen(float c)
    {
        Color newDelta = new Color(0, c, 0);

        lightSource.color += newDelta;
    }

    public void SubtractGreen(float c)
    {
        Color newDelta = new Color(0, c, 0);

        lightSource.color -= newDelta;
    }

    public void AddBlue(float c)
    {
        Color newDelta = new Color(0, 0, c);

        lightSource.color += newDelta;
    }

    public void SubtractBlue(float c)
    {
        AddBlue(-c);
    }


}
