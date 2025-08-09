using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TextUtility : MonoBehaviour
{
    public TextMeshPro textDisplay;
    public TextMeshProUGUI textDisplayPUGUI;
    public string buffer = "";
    public string cursorText = "_";

    void Start()
    {
        if (textDisplay == null)
            textDisplay = GetComponent<TextMeshPro>();

        if (textDisplayPUGUI == null)
            textDisplayPUGUI = GetComponent<TextMeshProUGUI>();
    }

    public void Append(string t)
    {
        buffer += t;

        if (textDisplay != null)
            textDisplay.text = buffer + cursorText;

        if (textDisplayPUGUI != null)
            textDisplayPUGUI.text = buffer + cursorText;
    }

    public void Backspace()
    {
        buffer = buffer.Substring(0, buffer.Length - 1);

        if (textDisplay != null)
            textDisplay.text = buffer + cursorText;

        if (textDisplayPUGUI != null)
            textDisplayPUGUI.text = buffer + cursorText;
    }
}

