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

    [Header("Event Handling")]
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;
    public UnityEvent onTriggerStay;
    public bool deleteOnEnter = false;
    public bool disableOnEnter = false;
    public GameObject subject;

    [Header("System Stuff - Usually Dont Touch")]
    public bool triggerActive = true;
    public Renderer myRenderer;
    [SerializeField]
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


    private void OnTriggerEnter(Collider other)
    {
        if (triggerActive)
        {
            if (CheckFilter(other))
            {
                if(disableOnEnter)
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

                onTriggerEnter.Invoke();
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (CheckFilter(other))
            onTriggerExit.Invoke();

        if (subject != null)
            subject = null;
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

        if(tf != null)
        {
            if(filterString.Length == 0)
            {
                return true;
            }
            else
            {
                return (filterString.Equals(tf.filterString));                    
            }
        }
        else
        {
            //objects without filters can trigger events
            if (allowUnfiltered)
                return true;
        }

        //otherwise - don't trigger events
        return false;
    }
}
