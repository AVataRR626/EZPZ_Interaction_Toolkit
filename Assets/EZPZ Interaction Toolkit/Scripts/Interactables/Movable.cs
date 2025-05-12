using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Movable : InteractableGeneral
{
    [Header("Movable Object Settings")]
    public UnityEvent onDrop;
    public Transform attachPoint;
    public float throwForce = 0;
    public bool noCollideOnHold = true;
    public bool groundPlace = false;
    public Vector3 groundPlaceOffset;
    public bool moving = false;
    public bool freezeRotation = false;           
    public Collider [] subCollliders;
    public float snapSpeed = 20;

    [Header("System Stuff (Usually Don't Touch)")]
    public Vector3 startingPosition;
    public Quaternion startingRotation;
    public Rigidbody myRbody;
    public Collider myCollider;
    public MovableMagnetSnapper myMagnetSnapper;
    public RaycastInteractor myRayManipulator;

    private void Start()
    {
        startingPosition = transform.position;
        startingRotation = transform.rotation;

        myCollider = GetComponent<Collider>();
        myRbody = GetComponent<Rigidbody>();

        if(subCollliders.Length <= 0)
        {
            subCollliders = transform.GetComponentsInChildren<Collider>();
        }

        if(attachPoint == null)
        {
            attachPoint =  transform.Find("attachPoint");
        }

        if (attachPoint == null)
        {
            attachPoint = transform.Find("AttachPoint");
        }

        if (attachPoint == null)
        {
            attachPoint = transform.Find("Attach Point");
        }

        if (attachPoint == null)
        {
            attachPoint = transform.Find("attach point");
        }

        if(attachPoint != null)
        {
            if(transform.localScale.x != 1 ||
                transform.localScale.y != 1 ||
                transform.localScale.z != 1)
            {
                Debug.LogError("!! Movable object with custom attach point does not have unit scale: " + name);
            }
        }

    }

    public void Update()
    {
        if(freezeRotation)
        {
            transform.rotation = startingRotation;
        }
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
            myRbody.linearVelocity = Vector3.zero;
            myRbody.angularVelocity = Vector3.zero;
            myRbody.useGravity = true;
        }
    }

    public void ResetOrientationAll()
    {   

        Movable[] allMovables = Object.FindObjectsByType<Movable>(FindObjectsSortMode.None);

        foreach (Movable m in allMovables)
            m.ResetOrientation();

        MovableMagnetSnapper[] allMagnets = Object.FindObjectsByType<MovableMagnetSnapper>(FindObjectsSortMode.None);

        if(allMagnets.Length > 0)
        {
            foreach (MovableMagnetSnapper m in allMagnets)
                m.ReleaseSubject();
        }

        Debug.Log("MOVABLE - RESET ALL");
    }


    public void Grab(RaycastInteractor newManipulator)
    {
        myRayManipulator = newManipulator;
    }
    
    public void Drop()
    {
        onDrop.Invoke();
    }

    public void ForceDrop()
    {
        moving = false;
        transform.parent = null;

        if(myRayManipulator != null)
        {
            myRayManipulator.previousMoveParent = null;
            myRayManipulator.moveSubject = null;
            myRayManipulator = null;
        }

        if(myMagnetSnapper != null)
        {
            myMagnetSnapper.subject = null;
            myMagnetSnapper = null;
        }

        
        SetColliderIsTrigger(this, false);        


        if(myRbody != null)
        {
            myRbody.isKinematic = false;
            myRbody.useGravity = true;
        }

        Drop();
    }

    public static void SetColliderIsTrigger(Movable m, bool setting)
    {
        Collider c = m.GetComponent<Collider>();
        if (c != null)
        {
            c.isTrigger = setting;

            if (m.subCollliders.Length > 0)
            {
                foreach (Collider subC in m.subCollliders)
                {
                    if (subC != null)
                    {
                        //exempt special object types
                        CharacterController cc = subC.GetComponent<CharacterController>();
                        InteractableTrigger it = subC.GetComponent<InteractableTrigger>();
                        MovableMagnetSnapper ms = subC.GetComponent<MovableMagnetSnapper>();

                        if (cc == null && it == null && ms == null)
                            subC.isTrigger = setting;
                    }
                }
            }
        }
    }

}