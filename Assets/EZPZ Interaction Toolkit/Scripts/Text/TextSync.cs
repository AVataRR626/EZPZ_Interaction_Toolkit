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
        //check for source text
        if (sourceDisplay != null)
        {
            text = sourceDisplay.text;
        }

        //prioritise TMPG
        if (sourceDisplayTMPG != null)
        {
            
            text = sourceDisplayTMPG.text;
            //text = "New String";
            Debug.Log("TextSync: " + sourceDisplayTMPG.text + " | " + text);
        }

        //sync to displays
        if (textDisplay != null)
        {
            textDisplay.text = text;
        }

        if (textDisplayTMPG != null)
        {

            textDisplayTMPG.text = text;
        }
    }

    public void SetText(string newText)
    {
        text = newText;
    }
}
