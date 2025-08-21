//EZPZ Interaction Toolkit
//by Matt Cabanag
//Created - don't remember when, some time in 2022-2923

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class NMAWalkTowards : MonoBehaviour
{

    [Header("Navigation")]
    public Transform destination;
    public UnityEvent onArrive;
    public UnityEvent onFirstMove;
    public Transform[] altDestinations;

    [Header("Graphics")]
    public Animator avatarAnimator;
    public string speedString = "speed";

    [Header("System Stuff (usually dont touch")]
    public NavMeshAgent myNma;
    public bool arrivalFlag = false;
    public bool prevArrivalFlag = false;
    public float dist2PrevPos;
    public Vector3 prevPos;
    public float fixedUpdateSpeed;
    public Vector3 fixedUpdateVelocity;
    public Vector3 prevPosFixedUpdate;

    // Start is called before the first frame update
    void Start()
    {
        if (myNma == null)
            myNma = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        dist2PrevPos = Vector3.Distance(transform.position, prevPos);

        if (dist2PrevPos > 0)
        {
            arrivalFlag = false;

            if (prevArrivalFlag != arrivalFlag)
            {
                onFirstMove.Invoke();
            }
        }
        else
        {
            arrivalFlag = true;

            if (prevArrivalFlag != arrivalFlag)
            {
                onArrive.Invoke();
            }
        }

        if (destination != null)
        {
            if(destination.gameObject.activeSelf)
                myNma.SetDestination(destination.position);
            else                
                myNma.SetDestination(transform.position);
        }

        HandleAvatar();

        prevArrivalFlag = arrivalFlag;
        prevPos = transform.position;
    }

    public void FixedUpdate()
    {
        fixedUpdateVelocity = myNma.velocity;
        fixedUpdateSpeed = myNma.velocity.magnitude;
        prevPosFixedUpdate = transform.position;
    }

    public void SetDestination(Transform newDest)
    {
        destination = newDest;
    }

    public void SetDestination(int i)
    {
        Debug.Log("Set Destination! " + i);
        destination = altDestinations[i];
    }

    [ContextMenu("Random Destination")]
    public void SetDestinationRandom()
    {
        int randomIndex = Random.Range(0, altDestinations.Length);

        while (altDestinations[randomIndex] == destination)
            randomIndex = Random.Range(0, altDestinations.Length);

        SetDestination(randomIndex);
    }

    public void HandleAvatar()
    {
        if (myNma != null)
        {   
            if (avatarAnimator != null)
            {
                avatarAnimator.SetFloat(speedString, fixedUpdateSpeed);
            }
        }
    }
}
