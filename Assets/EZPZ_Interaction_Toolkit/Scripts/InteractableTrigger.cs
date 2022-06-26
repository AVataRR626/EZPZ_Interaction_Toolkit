//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 21 Jul 2022

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
    public float cooldown = 0.25f;
    public float cooldownClock;    

    private void OnEnable()
    {
        triggerActive = false;
        Invoke("TriggerActive", 0.1f);
    }

    private void FixedUpdate()
    {
        if (cooldownClock > 0)
        {
            cooldownClock -= Time.fixedDeltaTime;
            triggerActive = false;
        }
        else
        {   
            TriggerActive();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerActive)
        {
            onTriggerEnter.Invoke();
            SetCooldown();
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (triggerActive)
        {
            onTriggerExit.Invoke();
            SetCooldown();
        }

        
    }

    
    private void OnTriggerStay(Collider other)
    {
        if (triggerActive)
        {
            onTriggerStay.Invoke();
            SetCooldown();
        }
    }
    

    public void TriggerActive()
    {
        cooldownClock = 0;
        triggerActive = true;
    }

    public void SetCooldown()
    {
        cooldownClock = cooldown;
        triggerActive = false;
    }
}
