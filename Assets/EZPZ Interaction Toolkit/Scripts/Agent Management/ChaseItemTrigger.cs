//EZPZ Interaction Toolkit
//by Matt Cabanag
//created Mar 2025

using UnityEngine;


public class ChaseItemTrigger : InteractableTrigger
{
    [Header("Agent Settings")]
    public NMAWalkTowards chaser;
    public bool autochase = true;

    private void Start()
    {
        if(autochase)
            onTriggerEnter.AddListener(Chase);
    }


    public void Chase()
    {
        Debug.Log("CHASE!");
        if(subject != null)
        {
            if(chaser != null)
            {
                chaser.destination = subject.transform;
            }
        }
    }
}
