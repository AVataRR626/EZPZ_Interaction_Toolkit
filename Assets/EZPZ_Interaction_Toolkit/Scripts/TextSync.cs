//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 11 Jul 2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextSync : MonoBehaviour
{
    public TextMeshPro textDisplay;
    public string text;

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
            textDisplay.text = text;
    }

    public void SetText(string newText)
    {
        text = newText;
    }
}
