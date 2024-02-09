using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NMAWalkTowards : MonoBehaviour
{    
    public Transform destination;

    public Transform[] altDestinations;

    [Header("System Stuff (usually dont touch")]
    public NavMeshAgent myNma;

    // Start is called before the first frame update
    void Start()
    {
        if (myNma == null)
            myNma = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (destination != null)
        {
            if(destination.gameObject.activeSelf)
                myNma.SetDestination(destination.position);
            else                
                myNma.SetDestination(transform.position);
        }
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
