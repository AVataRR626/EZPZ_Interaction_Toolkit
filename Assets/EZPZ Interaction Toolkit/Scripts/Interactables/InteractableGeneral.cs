//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 8 Apr 2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InteractableGeneral : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler //, , , IPointerClickHandler
{
    public UnityEvent onPrimaryInteract;
    public UnityEvent onHoverEnter;
    public UnityEvent onHoverExit;
    //public UnityEvent onHoldInteract;

    [Header("For Legacy Compatibility")]
    [Tooltip("This event is the same as onPrimaryInteract. Use that instead. This is only here to keep old things from breaking.")]
    public UnityEvent onFirstInteract;

    [Header("Advanced Setting - Be Careful!")]
    public InteractableGeneral eventRelay;
    

    private void Start()
    {
        onPrimaryInteract.AddListener(ParentPulse);
        onHoverEnter.AddListener(ParentPulseUp);
        onHoverExit.AddListener(ParentPulseDown);

        onFirstInteract.AddListener(ParentPulse);

        if (eventRelay != null)
        {
            onPrimaryInteract.AddListener(RelayOnPrimaryInteract);
            onHoverEnter.AddListener(RelayOnHoverEnter);
            onHoverExit.AddListener(RelayOnHoverExit);
            onFirstInteract.AddListener(RelayOnFirstInteract);
        }
    }

    public void PulseUp()
    {
        ScalePulse p = GetComponent<ScalePulse>();

        if(p != null)
        {
            p.PulseUp();
        }
    }

    public void PulseDown()
    {
        ScalePulse p = GetComponent<ScalePulse>();

        if (p != null)
        {
            p.PulseDown();
        }
    }

    public void Pulse()
    {
        ScalePulse p = GetComponent<ScalePulse>();

        if (p != null)
        {
            p.Pulse();
        }
    }

    public void ParentPulseUp()
    {
        if(transform.parent != null)
            gameObject.transform.parent.SendMessage("PulseUp", SendMessageOptions.DontRequireReceiver);
    }

    public void ParentPulseDown()
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

    public void RelayOnPrimaryInteract()
    {
        if (eventRelay != null)
            eventRelay.onPrimaryInteract.Invoke();
    }

    public void RelayOnFirstInteract()
    {
        if (eventRelay != null)
            eventRelay.onFirstInteract.Invoke();
    }

    public void RelayOnHoverEnter()
    {
        if (eventRelay != null)
            eventRelay.onHoverEnter.Invoke();
    }

    public void RelayOnHoverExit()
    {
        if (eventRelay != null)
            eventRelay.onHoverExit.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onPrimaryInteract.Invoke();
        Pulse();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter: " + gameObject.name);        
        onHoverEnter.Invoke();
        PulseUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointerExit: " + gameObject.name);
        onHoverExit.Invoke();
        PulseDown();
    }
}
