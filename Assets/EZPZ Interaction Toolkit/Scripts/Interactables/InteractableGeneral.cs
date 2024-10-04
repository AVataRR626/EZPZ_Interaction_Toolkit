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

    private void Start()
    {
        onFirstInteract.AddListener(ParentPulse);
        onHoverEnter.AddListener(ParentPulseUp);
        onHoverExit.AddListener(ParentPulseDOwn);
    }

    public void ParentPulseUp()
    {
        if(transform.parent != null)
            gameObject.transform.parent.SendMessage("PulseUp", SendMessageOptions.DontRequireReceiver);
    }

    public void ParentPulseDOwn()
    {
        if (transform.parent != null)
            gameObject.transform.parent.SendMessage("PulseDown", SendMessageOptions.DontRequireReceiver);
    }

    public void ParentPulse()
    {
        if (transform.parent != null)
            gameObject.transform.parent.SendMessage("Pulse", SendMessageOptions.DontRequireReceiver);
    }

    public void LoadScene(string newScene)
    {
        GenUtils.LoadScene(newScene);
    }
}
