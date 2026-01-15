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

        if(newParent.localScale != Vector3.one)
        {
            Debug.LogError("--WARNING:" + name + " : newParent local scale is not set to ( 1, 1, 1). You might introduce unstable behaviour.");
        }
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
                HoldableMagnetSnapper mSnap = subject.GetComponent<HoldableMagnetSnapper>();
                if (mSnap != null)
                    return;

                
                Holdable movable = subject.GetComponent<Holdable>();
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
                    if(pt.subject == other.gameObject)
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
