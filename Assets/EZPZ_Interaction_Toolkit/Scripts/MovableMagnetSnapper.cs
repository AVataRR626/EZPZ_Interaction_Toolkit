//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 28 Nov Jun 2023

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovableMagnetSnapper : MonoBehaviour
{
    [Header("Snap Settings")]
    public Transform snappingPoint;
    public bool alignRotation = true;

    [Header("Event Handling")]
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;
    public UnityEvent onTriggerStay;

    [Header("System Stuff (Usually Don't Touch)")]
    public Movable subject;

    // Start is called before the first frame update
    void Start()
    {
        if (snappingPoint == null)
            snappingPoint = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(subject != null)
        {
            if(!subject.moving)
            {
                subject.transform.parent = snappingPoint;
                subject.transform.localPosition = Vector3.zero;
                subject.transform.rotation = snappingPoint.rotation;

                Rigidbody r = subject.GetComponent<Rigidbody>();

                if(r != null)
                {
                    r.velocity = Vector3.zero;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        subject = other.GetComponent<Movable>();

        onTriggerEnter.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        subject = null;

        onTriggerExit.Invoke();
    }


    private void OnTriggerStay(Collider other)
    {
        onTriggerStay.Invoke();
    }
}
