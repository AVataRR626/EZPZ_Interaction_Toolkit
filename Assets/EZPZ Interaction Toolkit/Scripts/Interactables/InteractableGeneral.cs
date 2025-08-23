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
    [Header("Interaction Events - On Press")]
    [Tooltip("In desktop mode: this is for when you press your mouse button (not release)")]
    public UnityEvent onPrimaryInteract;
    [Tooltip("In desktop mode: this is for when you press [F] (not release)")]
    public UnityEvent onSecondaryInteract;

    [Header("Interaction Events - On Lift (or release)")]
    [Tooltip("In desktop mode: this is for when you press release mouse button")]
    public UnityEvent onPrimaryInteractLift;
    [Tooltip("In desktop mode: this is for when you release [F]")]
    public UnityEvent onSecondaryInteractLift;

    [Header("Interaction Customisations")]
    [Tooltip("Set to 0 or less to use default set on RaycastInteractor.")]
    public float customTouchDistance = -1;
    [Tooltip("Set to 0 or less to use default set on RaycastInteractor")]
    public float customHoldDistance = -1;
    [Tooltip("Block secondary events unless item is being held. Good for 'eating' type interactions.")]
    public bool restrictSecondaryToHeldOnly;
    public string heldText;

    [Header("Hover Settings")]
    public UnityEvent onHoverEnter;
    public UnityEvent onHoverExit;
    //public UnityEvent onHoldInteract;
    public string hoverText;

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
            customTouchDistance = eventRelay.customTouchDistance;
            customHoldDistance = eventRelay.customHoldDistance;
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

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }
}
