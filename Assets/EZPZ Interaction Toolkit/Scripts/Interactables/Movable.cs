using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : InteractableGeneral
{
    public bool noCollideOnHold = true;
    public bool groundPlace = false;
    public bool moving = false;
    public float throwForce = 0;
    public Rigidbody myRbody;

    [Header("System Stuff (Usually Don't Touch")]
    public Vector3 startingPosition;
    public Quaternion startingRotation;

    private void Start()
    {
        startingPosition = transform.position;
        startingRotation = transform.rotation;

        myRbody = GetComponent<Rigidbody>();
    }


    public void RotateY(float angle)
    {
        transform.Rotate(0, angle, 0);
    }

    public void ResetOrientation()
    {
        transform.position = startingPosition;
        transform.rotation = startingRotation;

        if(myRbody != null)
        {
            myRbody.velocity = Vector3.zero;
            myRbody.angularVelocity = Vector3.zero;
            myRbody.useGravity = true;
        }
    }

    public void ResetOrientationAll()
    {
        Movable[] allMovables = FindObjectsOfType<Movable>();

        foreach (Movable m in allMovables)
            m.ResetOrientation();

        MovableMagnetSnapper[] allMagnets = FindObjectsOfType<MovableMagnetSnapper>();

        if(allMagnets.Length > 0)
        {
            foreach (MovableMagnetSnapper m in allMagnets)
                m.ReleaseSubject();
        }
    }

}