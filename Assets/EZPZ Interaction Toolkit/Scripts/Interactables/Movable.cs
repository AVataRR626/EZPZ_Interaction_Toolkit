using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Movable : InteractableGeneral
{
    [Header("Movable Object Settings")]
    public UnityEvent onDrop;
    public float throwForce = 0;
    public bool noCollideOnHold = true;
    public bool groundPlace = false;
    public Vector3 groundPlaceOffset;
    public bool moving = false;
    public bool freezeRotation = false;
    public bool trayMode = false;           
    public Collider [] subCollliders;
    public float snapSpeed = 20;
    

    [Header("System Stuff (Usually Don't Touch)")]
    public Vector3 startingPosition;
    public Quaternion startingRotation;
    public Rigidbody myRbody;
    public Collider myCollider;
    public Movable myTray;
    public Vector3 trayOffset;    
    public List<Movable> trayInventory = new List<Movable>();
    public RaycastInteractor rayManipulator;

    private new void Start()
    {
        startingPosition = transform.position;
        startingRotation = transform.rotation;

        myCollider = GetComponent<Collider>();
        myRbody = GetComponent<Rigidbody>();
        trayInventory = new List<Movable>();

        if(subCollliders.Length <= 0)
        {
            subCollliders = transform.GetComponentsInChildren<Collider>();
        }
    }

    public void Update()
    {
        if(freezeRotation)
        {
            transform.rotation = startingRotation;
        }

        if(trayMode && trayInventory != null)
        {
            foreach (Movable m in trayInventory)
            {
                if (m != null)
                {
                    m.transform.position = transform.position + m.trayOffset;

                    if (!m.myRbody.isKinematic)
                    {
                        m.myRbody.linearVelocity = Vector3.zero;
                        m.myRbody.angularVelocity = Vector3.zero;
                        m.myRbody.isKinematic = true;
                    }

                    if (moving)
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
            myRbody.linearVelocity = Vector3.zero;
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

        Debug.Log("MOVABLE - RESET ALL");
    }

    
    public void OnCollisionEnter(Collision collision)
    {
        Movable otherMovable = collision.gameObject.GetComponent<Movable>();

        if(otherMovable != null)
        {
            if(otherMovable.trayMode)
            {
                //StartCoroutine(DAttachToTray(otherMovable,0.1f));
                if(transform.position.y > otherMovable.transform.position.y)
                    otherMovable.AttachToTray(this);
                
            }
            else if(trayMode)
            {
                myRbody.linearVelocity = Vector3.zero;
            }

        }
    }

    public IEnumerator DAttachToTray(Movable m, float s)
    {
        yield return new WaitForSeconds(s);

        AttachToTray(m);
    }

    public void AttachToTray(Movable m)
    {
        if (m != null && trayInventory != null)
        {   
            if (!trayInventory.Contains(m))
            {
                trayInventory.Add(m);

                m.myTray = this;
                m.trayOffset = m.transform.position - transform.position + new Vector3(0, 0.01f, 0);
                m.myRbody.linearVelocity = Vector3.zero;
                m.myRbody.angularVelocity = Vector3.zero;
                m.myRbody.useGravity = false;
                m.transform.rotation = m.startingRotation;
            }
        }
    }

    public void Grab(RaycastInteractor newManipulator)
    {
        rayManipulator = newManipulator;

        if (myTray != null)
        {
            if(myTray.trayInventory !=  null)
                myTray.trayInventory.Remove(this);

            myTray = null;
        }
    }
    
    public void Drop()
    {
        //Debug.Log("--- DROP");

        if(trayMode && trayInventory != null)
        {
            foreach (Movable m in trayInventory)
            {
                if (m != null)
                {
                    //m.transform.position = transform.position + m.trayOffset;
                    m.myRbody.useGravity = true;
                    m.myRbody.isKinematic = false;
                    m.myCollider.enabled = true;
                }
            }

            trayInventory.Clear();
        }

        if (myTray != null)
        {
            if (myTray.trayInventory != null)            
                myTray.trayInventory.Remove(this);

            myTray = null;
        }

        onDrop.Invoke();
    }

    public void ForceDrop()
    {
        moving = false;
        transform.parent = null;

        if(rayManipulator != null)
        {
            rayManipulator.previousMoveParent = null;
            rayManipulator.moveSubject = null;
        }
        
        SetColliderIsTrigger(this, false);        
        myCollider.enabled = true;
        myTray = null;

        if(myRbody != null)
        {
            myRbody.isKinematic = false;
            myRbody.useGravity = true;
        }
    }

    public void ResetTray()
    {
        foreach (Movable m in trayInventory)
        {
            if (m != null)
            {
                m.myCollider.enabled = true;
                m.myTray = null;
                m.myRbody.isKinematic = false;
            }
        }

        trayInventory.Clear();
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
                        CharacterController cc = subC.GetComponent<CharacterController>();
                        InteractableTrigger it = subC.GetComponent<InteractableTrigger>();
                        if (cc == null && it == null)
                            subC.isTrigger = setting;
                    }
                }
            }
        }
    }

}