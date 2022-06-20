//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 21 Jul 2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableTrigger : MonoBehaviour
{
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;
    public UnityEvent onTriggerStay;

    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnter.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        onTriggerExit.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        onTriggerStay.Invoke();
    }
}
