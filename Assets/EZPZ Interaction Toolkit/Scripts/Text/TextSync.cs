//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 11 Jul 2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextSync : MonoBehaviour
{
    public string text;

    [Header("TextMeshProGUI")]
    public TextMeshProUGUI textDisplayTMPG;
    public TextMeshProUGUI sourceDisplayTMPG;

    [Header("TextMeshPro")]
    public TextMeshPro textDisplay;    
    public TextMeshPro sourceDisplay;

    // Start is called before the first frame update
    void Start()
    {
        if (textDisplay == null)
            textDisplay = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        if (textDisplay != null)
        {
            if (sourceDisplay != null)
                textDisplay.text = sourceDisplay.text;
            else
                textDisplay.text = text;
        }

        if (textDisplayTMPG != null)
        {
            if (sourceDisplayTMPG != null)
                textDisplayTMPG.text = sourceDisplayTMPG.text;
            else
                textDisplayTMPG.text = text;
        }
    }

    public void SetText(string newText)
    {
        text = newText;
    }
}
