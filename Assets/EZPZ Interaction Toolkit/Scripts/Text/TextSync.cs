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

    [Header("Source Display")]
    public TextMeshProUGUI sourceDisplayUGUI;
    public TextMeshPro sourceDisplay;
    

    [Header("Sync Display")]
    public TextMeshProUGUI textDisplayUGUI;
    public TextMeshPro textDisplay;
    
    

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
        if (sourceDisplayUGUI != null)
        {
            
            text = sourceDisplayUGUI.text;
            //text = "New String";
            //Debug.Log("TextSync: " + sourceDisplayTMPG.text + " | " + text);
        }

        //sync to displays
        if (textDisplay != null)
        {
            textDisplay.text = text;
        }

        if (textDisplayUGUI != null)
        {

            textDisplayUGUI.text = text;
        }
    }

    public void SetText(string newText)
    {
        text = newText;
    }
}
