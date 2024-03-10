using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class NMAWalkTowards : MonoBehaviour
{    
    public Transform destination;

    public UnityEvent onArrive;
    public UnityEvent onFirstMove;

    public Transform[] altDestinations;

    [Header("System Stuff (usually dont touch")]
    public NavMeshAgent myNma;
    public bool arrivalFlag = false;
    public bool prevArrivalFlag = false;
    public float dist2PrevPos;
    public Vector3 prevPos;

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
        


        prevArrivalFlag = arrivalFlag;
        prevPos = transform.position;
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
}
