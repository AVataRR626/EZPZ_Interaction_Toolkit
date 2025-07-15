//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 10 Mar 2024

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchAreaTrigger : InteractableTrigger
{   
    public List<Rigidbody> subjectList;
    public Transform launchDirection;
    public Transform launchPoint;
    public float forceFactor = 10;

    public void OnTriggerEnter(Collider other)
    {
        Rigidbody r = other.gameObject.GetComponent<Rigidbody>();

        if(r != null)
        {
            if(!subjectList.Contains(r))
                subjectList.Add(r);
        }

        onTriggerEnter.Invoke();
    }

    public void Launch()
    {
        Launch(1);
    }

    public void Launch(float launchForce)
    {
        foreach(Rigidbody r in subjectList)
        {
            Vector3 launchVector = new Vector3();

            if (launchDirection != null)
                launchVector = launchDirection.forward;
            else
                launchVector = transform.forward;

            if (launchPoint != null)
                subject.transform.position = launchPoint.position;


            r.AddForce(launchVector * launchForce * forceFactor);
        }

        subjectList.Clear();
    }
}
