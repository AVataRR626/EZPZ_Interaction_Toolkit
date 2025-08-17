//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 27 May 2025

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentItemTrigger : InteractableTrigger
{
    [Header("Parenting Settings")]
    public bool autoParent = true;
    public bool autoUnparent = true;
    public Transform newParent;

    [Header("System Stuff - Usually don't touch")]
    public Transform originalParent;
    public List<ParentTracker> originalParents = new List<ParentTracker>();

    [System.Serializable]
    public class ParentTracker
    {
        public GameObject subject;
        public Transform originalParent;
    }

    private void Start()
    {
        if(newParent == null)
            newParent = transform;
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

    public override void OnTriggerEnter(Collider other)
    {
        //Debug.Log("ParentItemTrigger.OnTriggerEnter()" + other.name);

        base.OnTriggerEnter(other);

        if(autoParent)
        {
            if(CheckFilter(other))
            {
                //don't parent magnet snappers!
                MovableMagnetSnapper mSnap = subject.GetComponent<MovableMagnetSnapper>();
                if (mSnap != null)
                    return;

                
                Movable movable = subject.GetComponent<Movable>();
                if (movable != null)
                {   //don't parent held objects
                    if(!movable.moving)
                        AttachToNewParent(subject);
                }
                else
                {
                    //normal operation...
                    AttachToNewParent(subject);
                }
            }
        }
    }

    public void AttachToNewParent(GameObject o)
    {
        if (contactList.Contains(o))
        {
            ParentTracker pt = new ParentTracker();
            pt.subject = o;
            pt.originalParent = o.transform.parent;
            originalParents.Add(pt);
            subject.transform.parent = newParent;
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        //Debug.Log("ParentItemTrigger.OnTriggerExit()" + other.name);

        base.OnTriggerEnter(other);

        if(autoUnparent)
        {
            if (CheckFilter(other))
            {
                ParentTracker found = null;

                foreach(ParentTracker pt in originalParents)
                {
                    if(pt.subject == subject)
                    {   
                        found = pt;
                    }
                }

                if(found != null)
                {
                    if (found.subject.transform.parent == newParent)
                    {
                        found.subject.transform.parent = found.originalParent;
                    }

                    originalParents.Remove(found);
                }
            }
        }

    }
}
