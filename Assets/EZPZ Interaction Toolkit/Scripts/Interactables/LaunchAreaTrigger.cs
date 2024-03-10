//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 10 Mar 2024

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchAreaTrigger : InteractableTrigger
{
    public Rigidbody subjectRBody;
    public Transform launchDirection;
    public Transform launchPoint;
    public float forceFactor = 10;

    public void Launch()
    {
        Launch(1);
    }

    public void Launch(float launchForce)
    {
        if (subject != null)
        {
            subjectRBody = subject.GetComponent<Rigidbody>();

            if (subjectRBody != null)
            {
                Vector3 launchVector = new Vector3();

                if (launchDirection != null)
                    launchVector = launchDirection.forward;
                else
                    launchVector = transform.forward;

                if (launchPoint != null)
                    subject.transform.position = launchPoint.position;


                subjectRBody.AddForce(launchVector * launchForce * forceFactor);
            }
        }
    }
}
