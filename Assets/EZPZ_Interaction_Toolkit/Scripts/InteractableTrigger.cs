//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 21 Jun 2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableTrigger : MonoBehaviour
{
    [Header("Filter Settings")]
    public string filterString = "";
    public bool allowUnfiltered = true;

    [Header("Events")]
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;
    public UnityEvent onTriggerStay;

    [Header("Cooldown Management")]
    public bool triggerActive = true;    

    private void OnEnable()
    {
        triggerActive = false;
        Invoke("TriggerActive", 0.1f);
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (triggerActive)
        {
            if(CheckFilter(other))
                onTriggerEnter.Invoke();            
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
