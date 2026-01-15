//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 21 Jun 2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableTrigger : MonoBehaviour
{
    public bool visibleAtRuntime = false;

    [Header("Filter Settings")]
    public string filterString = "";
    public bool allowUnfiltered = true;
    [Tooltip("")]
    public bool excludeMode = false;

    [Header("Event Handling")]
    public UnityEvent onTriggerEnter;
    [Tooltip("Contacts will be deleted if set to true")]
    public bool deleteOnEnter = false;
    [Tooltip("Contacts will be disabled if set to true")]
    public bool disableOnEnter = false;
    public UnityEvent onTriggerExit;
    public UnityEvent onTriggerStay;

    [Header("System Stuff - Usually Dont Touch")]
    public GameObject subject;
    public bool triggerActive = true;
    public Renderer myRenderer;
    public List<GameObject> contactList = new List<GameObject>();
    public ISubjectRelay subjectSync;

    private void OnEnable()
    {
        triggerActive = false;
        Invoke("TriggerActive", 0.1f);

        Collider c = GetComponent<Collider>();

        if(c != null)
        {
            c.isTrigger = true;
        }
        else
        {
            Debug.LogError("ERROR: Missing Collider on InteractableTrigger: " + name);
        }
    }

    private void Start()
    {
        myRenderer = GetComponent<Renderer>();

        if (myRenderer != null)
            myRenderer.enabled = visibleAtRuntime;
    }


    public virtual void OnTriggerEnter(Collider other)
    {
        if (triggerActive)
        {
            if (CheckFilter(other))
            {
                if (disableOnEnter)
                {
                    if (other.tag != "Player")
                        other.gameObject.SetActive(false);
                }
                else
                {
                    subject = other.gameObject;
                }

                if (deleteOnEnter)
                {
                    if (other.tag != "Player")
                        Destroy(other.gameObject);
                }
                else
                {
                    subject = other.gameObject;
                }

                if(subject != null)
                {
                    if(subjectSync != null)
                        subjectSync.SyncSubject(subject);
                }

                if (subject != null)
                    Add2ContactList(subject);

                onTriggerEnter.Invoke();
            }
        }
        
    }

    public void Add2ContactList(GameObject o)
    {
        if(!contactList.Contains(o))
        {
            contactList.Add(o);
        }
    }


    public virtual void OnTriggerExit(Collider other)
    {
        if (CheckFilter(other))
        {
            onTriggerExit.Invoke();
            RemoveFromContactList(other.gameObject);
        }

        if (subject != null)
            subject = null;
    }

    public void RemoveFromContactList(GameObject o)
    {
        if (contactList.Contains(o))
        {
            contactList.Remove(o);
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (CheckFilter(other))
            onTriggerStay.Invoke();
    }
    

    public void TriggerActive()
    {
        triggerActive = true;
    }

    public void SetCooldown()
    {
     
        triggerActive = false;
    }

    public bool CheckFilter(Collider other)
    {
        TriggerFilter tf = other.GetComponent<TriggerFilter>();
        bool result = false;

        if(tf != null)
        {
            if(filterString.Length == 0)
            {
                //allow any filter text when filter string is not set
                result = true;
            }
            else
            {
                //otherwise, check if the filter strings match
                result = (filterString.Equals(tf.filterString));                    
            }

            if (excludeMode)
                result = !result;
        }
        else
        {
            //when allowing unfiltered objects,
            //objects without trigger filters can trigger events regardless
            if (allowUnfiltered)
                result = true;
        }

        //invert result if exclude mode is active
        return result;
    }

    public void LoadScene(string newScene)
    {
        GenUtils.LoadScene(newScene);
    }
}
