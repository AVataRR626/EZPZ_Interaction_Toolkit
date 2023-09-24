using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class Typable : InteractableGeneral
{
    public UnityEvent onTextMatch;
    public string matchText;
    public string cursorText = "_";


    [Header("System Stuff - Usually Don't Touch")]
    public string typeTextBuffer;
    public bool typeCapture;
    public TextMeshProUGUI textDisplay;    

    public RaycastInteractor raycastInteractor;    

    private void Start()
    {
        Keyboard.current.onTextInput += OnTextInput;
    }

    private void OnTextInput(char ch)
    {
        if (typeCapture)
        {
            if (ch == '\b')
            {
                //backspace
                typeTextBuffer = typeTextBuffer.Substring(0, typeTextBuffer.Length - 1);
            }
            else if(ch == '\r')
            {
                //enter
                raycastInteractor.ReleaseFromTyping();
            }
            else
            {
                typeTextBuffer += ch;
            }

            SyncText();
        }
    }

    public void ClearTypeBuffer()
    {
        typeTextBuffer = "";
        SyncText();
    }

    public void SyncText()
    {
        if (typeCapture)
        {
            textDisplay.text = typeTextBuffer + cursorText;

            if (typeTextBuffer.Length > 0)
            {
                if (typeTextBuffer.Equals(matchText))
                {
                    onTextMatch.Invoke();

                    if (raycastInteractor != null)
                        raycastInteractor.ReleaseFromTyping();
                }
            }
        }
        else
            textDisplay.text = typeTextBuffer;
    }
}
