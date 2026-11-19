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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        counter++;


        if(counter == triggerCount)
        {
            onTriggerCountReached.Invoke();
        }

        onTriggerEnter.Invoke();
    }

    public void SetCounter(int c)
    {
        counter = c;
    }
}
