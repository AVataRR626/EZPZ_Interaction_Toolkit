//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 8 Apr 2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableGeneral : MonoBehaviour
{   
    public UnityEvent onFirstInteract;
    public UnityEvent onHoverEnter;
    public UnityEvent onHoverExit;
    //public UnityEvent onHoldInteract;

    public void LoadScene(string newScene)
    {
        GenUtils.LoadScene(newScene);
    }
}
