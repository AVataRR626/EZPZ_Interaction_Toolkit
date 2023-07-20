using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NMAWalkTowards : MonoBehaviour
{    
    public Transform destination;

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
        }
    }

    public void SetDestination(Transform newDest)
    {
        destination = newDest;
    }
}
