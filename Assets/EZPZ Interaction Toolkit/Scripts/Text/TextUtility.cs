using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TextUtility : MonoBehaviour
{
    public TextMeshPro textDisplay;
    public TextMeshProUGUI textDisplayPUGUI;

    void Start()
    {
        if (textDisplay == null)
            textDisplay = GetComponent<TextMeshPro>();

        if (textDisplayPUGUI == null)
            textDisplayPUGUI = GetComponent<TextMeshProUGUI>();
    }

    public void Append(string t)
    {
        if(textDisplay != null)
            textDisplay.text += t;

        if (textDisplayPUGUI != null)
            textDisplayPUGUI.text += t;
    }

    public void Backspace()
    {
        if (textDisplay != null)
            if (textDisplay.text.Length > 0)
                textDisplay.text = textDisplay.text.Substring(0, textDisplay.text.Length - 1);


        if (textDisplayPUGUI != null)
            if (textDisplayPUGUI.text.Length > 0)
                textDisplayPUGUI.text = textDisplayPUGUI.text.Substring(0, textDisplayPUGUI.text.Length - 1);
    }
}
