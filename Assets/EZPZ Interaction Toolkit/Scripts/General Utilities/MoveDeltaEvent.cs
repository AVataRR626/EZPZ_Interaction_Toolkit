//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 18 Apr 2025
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Events;

public class MoveDeltaEvent : MonoBehaviour
{
    public Transform subject;
    public float moveDeltaThreshold = 3;
    public UnityEvent onOverThreshold;
    public UnityEvent onUnderThreshold;

    [Header("System Stuff (usually don't touch")]
    public Vector3 prevPos;
    public float moveDelta;
    public bool overFlag = false;
    public bool underFlag = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (subject == null)
            subject = gameObject.transform;

        prevPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        moveDelta = Vector3.Distance(subject.position, prevPos);

        if (moveDelta >= moveDeltaThreshold)
        {
            if (!overFlag)
            {
                onOverThreshold.Invoke();
                overFlag = true;
                underFlag = false;
            }
        }
        else
        {
            if (!underFlag)
            {
                onUnderThreshold.Invoke();
                overFlag = false;
                underFlag = true;
            }
            
        }


        prevPos = transform.position;
    }
}
