//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 27 May 2025

using UnityEngine;

public class ParentItemTrigger : InteractableTrigger
{
    [Header("Parenting Settings")]
    public bool autoParent = true;
    public bool autoUnparent = true;
    public Transform newParent;

    [Header("System Stuff - Usually don't touch")]
    public Transform originalParent;

    private void Start()
    {
        if(newParent == null)
            newParent = transform;

        if (autoParent)
            onTriggerEnter.AddListener(ParentSubject);

        if (autoUnparent)
            onTriggerExit.AddListener(UnparentSubject);
    }

    public void ParentSubject()
    {
        if (subject != null)
        {
            originalParent = subject.transform.parent;
            subject.transform.parent = newParent;
        }
    }

    public void UnparentSubject()
    {
        if (subject != null)
        {   
            subject.transform.parent = originalParent;
        }
    }
}
