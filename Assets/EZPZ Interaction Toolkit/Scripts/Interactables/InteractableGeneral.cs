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

    public void Start()
    {
        onFirstInteract.AddListener(Pulse);
        onHoverEnter.AddListener(PulseUp);
        onHoverExit.AddListener(PulseDown);
    }

    public void PulseUp()
    {   
        gameObject.transform.parent.SendMessage("PulseUp", SendMessageOptions.DontRequireReceiver);
    }

    public void PulseDown()
    {
        gameObject.transform.parent.SendMessage("PulseDown", SendMessageOptions.DontRequireReceiver);
    }

    public void Pulse()
    {
        gameObject.transform.parent.SendMessage("Pulse", SendMessageOptions.DontRequireReceiver);
    }

    public void LoadScene(string newScene)
    {
        GenUtils.LoadScene(newScene);
    }
}
