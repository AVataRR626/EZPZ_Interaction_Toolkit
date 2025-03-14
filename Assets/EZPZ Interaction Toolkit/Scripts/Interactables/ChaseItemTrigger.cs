
using UnityEngine;

public class ChaseItemTrigger : InteractableTrigger
{
    [Header("Agent Settings")]
    public NMAWalkTowards chaser;

    private void Start()
    {
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
