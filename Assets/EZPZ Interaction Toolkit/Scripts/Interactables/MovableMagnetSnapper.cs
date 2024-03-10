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
    public UnityEvent onSnap;
    public UnityEvent onRelease;
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;
    public UnityEvent onTriggerStay;


    [Header("System Stuff (Usually Don't Touch)")]
    public Movable subject;
    public bool snapFlag = true;

    // Start is called before the first frame update
    void Start()
    {
        if (snappingPoint == null)
            snappingPoint = transform;

        snapFlag = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(subject != null)
        {
            if (!snapFlag)
            {
                if (!subject.moving)
                {
                    subject.transform.parent = snappingPoint;
                    subject.transform.localPosition = Vector3.zero;
                    subject.transform.rotation = snappingPoint.rotation;

                    Debug.Log("On Snap!");
                    onSnap.Invoke();

                    Rigidbody r = subject.GetComponent<Rigidbody>();

                    if (r != null)
                    {
                        r.velocity = Vector3.zero;
                        r.useGravity = false;
                    }

                    snapFlag = true;
                }
            }

            if(snapFlag && !subject.moving)
            {
                subject.transform.localPosition = Vector3.zero;
                subject.transform.rotation = snappingPoint.rotation;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (subject == null)
        {
            subject = other.GetComponent<Movable>();

            if (subject != null)
            {
                snapFlag = false;
            }
        }

        onTriggerEnter.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (subject != null)
        {
            Movable om = other.GetComponent<Movable>();

            if (om != null)
            {
                //make sure the object exiting the area
                //is exactly the one that's leaving...
                if (om == subject)
                {
                    ReleaseSubject();
                }
                //don't want another object to trigger dropping
            }
        }

        onTriggerExit.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        onTriggerStay.Invoke();
    }

    public void ReleaseSubject()
    {
        if(subject != null)
        {
            Debug.Log("ReleaseSubject");
            onRelease.Invoke();
            snapFlag = true;
            subject = null;
        }
    }
}
