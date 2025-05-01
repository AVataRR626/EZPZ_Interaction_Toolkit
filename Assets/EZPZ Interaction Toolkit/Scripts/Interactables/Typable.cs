using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using System.Security.Cryptography;
using System;

public class Typable : InteractableGeneral
{
    [Header("Typing Interaction Settings")]
    public UnityEvent onTextMatch;
    public UnityEvent onReleaseTyping;
    public UnityEvent onEnterKeyNotForWebGL;
    public TextMatchRelay textMatchRelay;
    public string matchText;
    public string cursorText = "_";
    public bool releaseOnEnterKey = true;    


    [Header("System Stuff - Usually Don't Touch")]
    public string typeTextBuffer;
    public bool typeCapture;
    public TextMeshProUGUI textDisplay;    

    public RaycastInteractor raycastInteractor;    

    private void Start()
    {
        Keyboard.current.onTextInput += OnTextInput;
    }

    public void OnMouseDown()
    {
        raycastInteractor.ReleaseFromTyping();
    }

    private void OnTextInput(char ch)
    {
        if (typeCapture)
        {
            if (ch == '\b')
            {
                //backspace
                if(typeTextBuffer.Length >= 1)
                    typeTextBuffer = typeTextBuffer.Substring(0, typeTextBuffer.Length - 1);
            }
            else if (ch == 127)
            {
                Debug.Log("MAC backspace");
                //backspace
                if (typeTextBuffer.Length >= 1)
                    typeTextBuffer = typeTextBuffer.Substring(0, typeTextBuffer.Length - 1);
            }
            else if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                Debug.Log("enterKey.wasPressed");
                HandleEnterKey();
            }
            //else if(ch == '\r')
            //{
            //    HandleEnterKey();
            //}
            else if(ch == '')
            {
                raycastInteractor.ReleaseFromTyping();
                //_  <- the escape key string
            }
            else if(ch == '`')
            {
                onReleaseTyping.Invoke();
                raycastInteractor.ReleaseFromTyping();
                //tab: '\t'
                //tab: '\x09'
            }
            else
            {
                typeTextBuffer += ch;
            }

            SyncText();
        }
    }

    public void HandleEnterKey()
    {
        if (releaseOnEnterKey)
            raycastInteractor.ReleaseFromTyping();
        else
            typeTextBuffer += '\n';

        onEnterKeyNotForWebGL.Invoke();
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

            if(textMatchRelay != null)
                textMatchRelay.CheckMatch();
        }
        else
            textDisplay.text = typeTextBuffer;
    }
 }

