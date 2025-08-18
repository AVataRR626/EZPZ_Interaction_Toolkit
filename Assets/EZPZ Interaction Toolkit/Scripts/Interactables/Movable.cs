using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Movable : InteractableGeneral
{
    [Header("Movable Object Settings")]
    public UnityEvent onDrop;
    public Transform attachPoint;
    public float throwForce = 0;
    public bool noCollideOnHold = true;
    [Tooltip("Set to 'true' if you want the object to hug the environment.")]
    public bool groundPlace = false;
    [Tooltip("Set to 'true' if you want to automatically drop objects when you release the mouse button.")]
    public bool dropOnKeyLift = false;
    public Vector3 groundPlaceOffset;    
    public bool freezeRotation = false;               
    public float snapSpeed = 20;

    [Header("System Stuff (Usually Don't Touch)")]
    public Vector3 startingPosition;
    public Quaternion startingRotation;
    public Rigidbody myRbody;
    public Collider myCollider;
    public MovableMagnetSnapper myMagnetSnapper;
    public RaycastInteractor myRayManipulator;
    public bool originalUseGravity = true;
    public bool moving = false;
    public Collider[] subCollliders;

    private void Start()
    {
        startingPosition = transform.position;
        startingRotation = transform.rotation;

        myCollider = GetComponent<Collider>();
        myRbody = GetComponent<Rigidbody>();

        if(myRbody != null)
        {
            originalUseGravity = myRbody.useGravity;
        }

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
                Debug.LogError("!!!! Alert !!!! (" + name + ") Object with custom attachpoint does not have unit scale. This will cause object pickup & drop problems. Reset scale to(1, 1, 1)");
            }
        }
    }

    public void Update()
    {
        if(freezeRotation)
        {
            transform.rotation = startingRotation;

            
        }

        if (myMagnetSnapper != null)
        {
            if(myMagnetSnapper.subject != this)
            {
                ForceDrop();
            }
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
        {
            m.ResetOrientation();
            m.transform.parent = null;
        }

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
            myRbody.useGravity = originalUseGravity;
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