using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Movable : InteractableGeneral
{
    public UnityEvent onDrop;
    public bool noCollideOnHold = true;
    public bool groundPlace = false;
    public bool moving = false;
    public bool freezeRotation = false;
    public bool trayMode = false;       
    public float throwForce = 0;
    

    [Header("System Stuff (Usually Don't Touch")]
    public Vector3 startingPosition;
    public Quaternion startingRotation;
    public Rigidbody myRbody;
    public Collider myCollider;
    public Movable myTray;
    public Vector3 trayOffset;    
    public List<Movable> trayInventory;

    private void Start()
    {
        startingPosition = transform.position;
        startingRotation = transform.rotation;

        myCollider = GetComponent<Collider>();
        myRbody = GetComponent<Rigidbody>();
        trayInventory = new List<Movable>();
    }

    public void Update()
    {
        if(freezeRotation)
        {
            transform.rotation = startingRotation;
        }

        if(trayMode)
        {
            if (moving)
            {
                foreach (Movable m in trayInventory)
                {
                    m.transform.position = transform.position + m.trayOffset;
                    m.myRbody.isKinematic = true;
                    m.myCollider.enabled = false;
                }
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

    
    public void OnCollisionEnter(Collision collision)
    {
        Movable otherMovable = collision.gameObject.GetComponent<Movable>();

        if(otherMovable != null)
        {
            if(trayMode)
            {
                trayInventory.Add(otherMovable);

                otherMovable.myTray = this;
                otherMovable.trayOffset = otherMovable.transform.position - transform.position;
                //otherMovable.myRbody.velocity = Vector3.zero;
                otherMovable.myRbody.angularVelocity = Vector3.zero;               
                otherMovable.transform.rotation = otherMovable.startingRotation;
                //transform.parent = otherMovable.transform;
            }
        }
    }

    public void Grab()
    {

    }
    
    public void Drop()
    {
        //Debug.Log("--- DROP");

        if(trayMode)
        {
            ResetTray();            
            transform.position = transform.position + new Vector3(0, -0.1f, 0);
            //Debug.Log("-x-x-x-x- DROP TRAY");
        }

        if (myTray != null)
        {
            myTray.trayInventory.Remove(this);
            myTray = null;
        }

        onDrop.Invoke();
    }

    public void ResetTray()
    {
        foreach (Movable m in trayInventory)
        {
            m.myCollider.enabled = true;
            m.myTray = null;
            m.myRbody.isKinematic = false;
        }

        trayInventory.Clear();
    }

}