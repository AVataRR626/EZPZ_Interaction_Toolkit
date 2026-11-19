//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 19 Nov 2026

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEnterCountEvent : MonoBehaviour
{
    public int counter;
    public int triggerCount;

    public UnityEvent onTriggerCountReached;

    public UnityEvent onTriggerEnter;

    [Header("System Stuff - Usually Don't Touch")]
    public bool triggerActive = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        counter = 0;

        Collider c = GetComponent<Collider>();

        if (c != null)
        {
            c.isTrigger = true;
        }
        else
        {
            Debug.LogError("ERROR: Missing Collider on InteractableTrigger: " + name);
        }

        triggerActive = false;
        Invoke("TriggerActive", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (triggerActive)
        {
            counter++;


            if (counter == triggerCount)
            {
                onTriggerCountReached.Invoke();
            }

            onTriggerEnter.Invoke();
        }
    }

    public void SetCounter(int c)
    {
        counter = c;
    }

    public void TriggerActive()
    {
        triggerActive = true;
    }
}
