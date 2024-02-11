//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 21 Jun 2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableTrigger : MonoBehaviour
{
    public bool visibleAtRuntime = false;

    [Header("Filter Settings")]
    public string filterString = "";
    public bool allowUnfiltered = true;

    [Header("Event Handling")]
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;
    public UnityEvent onTriggerStay;
    public bool deleteOnEnter = false;

    [Header("System Stuff - Usually Dont Touch")]
    public bool triggerActive = true;
    public Renderer myRenderer;

    private void OnEnable()
    {
        triggerActive = false;
        Invoke("TriggerActive", 0.1f);
    }

    private void Start()
    {
        myRenderer = GetComponent<Renderer>();

        if (myRenderer != null)
            myRenderer.enabled = visibleAtRuntime;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (triggerActive)
        {
            if (CheckFilter(other))
            {
                onTriggerEnter.Invoke();

                if (deleteOnEnter)
                {
                    if (other.tag != "Player")
                        Destroy(other.gameObject);
                }
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (CheckFilter(other))
            onTriggerExit.Invoke();        
    }

    
    private void OnTriggerStay(Collider other)
    {
        if (CheckFilter(other))
            onTriggerStay.Invoke();
    }
    

    public void TriggerActive()
    {
        triggerActive = true;
    }

    public void SetCooldown()
    {
     
        triggerActive = false;
    }

    public bool CheckFilter(Collider other)
    {
        TriggerFilter tf = other.GetComponent<TriggerFilter>();

        if(tf != null)
        {
            if(filterString.Length == 0)
            {
                return true;
            }
            else
            {
                return (filterString.Equals(tf.filterString));                    
            }
        }
        else
        {
            //objects without filters can trigger events
            if (allowUnfiltered)
                return true;
        }

        //otherwise - don't trigger events
        return false;
    }
}
