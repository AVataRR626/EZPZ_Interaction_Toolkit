//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 21 Jun 2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableTrigger : MonoBehaviour
{
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
            onTriggerEnter.Invoke();            
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        onTriggerExit.Invoke();        
    }

    
    private void OnTriggerStay(Collider other)
    {
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
}
