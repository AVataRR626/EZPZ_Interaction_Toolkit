using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtRotate : MonoBehaviour
{
    public Transform target;
    public Transform subject;
    public string defaultTargetTag = "Player";
    public bool constantTracking = true;
    public bool yLookOnly = true;

    [Header("System Stuff")]
    public Quaternion startRotation;
    public float yRotation;

    private void Start()
    {
        if (subject == null)
            subject = transform;

        if(target == null)
        {
            GameObject candidate = GameObject.FindGameObjectWithTag(defaultTargetTag);
            if (candidate != null)
                target = candidate.transform;
        }

        if (subject == null)
            startRotation = transform.rotation;
        else
            startRotation = subject.rotation;
    
    
    }

    // Update is called once per frame
    void Update()
    {
        if(constantTracking)
        {
            TrackTarget();
        }
    }

    public void TrackTarget()
    {
        if (target != null)
        {
            subject.LookAt(target);
            yRotation = subject.rotation.eulerAngles.y;

            if (yLookOnly)
            {
                transform.rotation = startRotation;
                transform.Rotate(0, yRotation, 0);
            }

        }
    }

    public void TrackTarget(bool mode)
    {
        constantTracking = mode;

        if(mode)
        {
            startRotation = Quaternion.identity;
        }
    }
}
