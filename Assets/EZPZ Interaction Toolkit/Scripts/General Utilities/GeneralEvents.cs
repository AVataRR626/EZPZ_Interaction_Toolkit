//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 17 Aug 2025

using UnityEngine;
using UnityEngine.Events;

public class GeneralEvents : MonoBehaviour
{
    public UnityEvent onStart;
    public UnityEvent onEnable;
    public UnityEvent onDisable;
    public UnityEvent onUpdate;
    public UnityEvent onCollisionEnter;
    public UnityEvent onCollisionExit;
    public UnityEvent onCollisionStay;
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;
    public UnityEvent onTriggerStay;
    
    void Start()
    {
        onStart.Invoke();
    }

    private void OnEnable()
    {
        onEnable.Invoke();
    }

    private void OnDisable()
    {
        onDisable.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        onUpdate.Invoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        onCollisionEnter.Invoke();
    }

    private void OnCollisionExit(Collision collision)
    {
        onCollisionExit.Invoke();
    }

    private void OnCollisionStay(Collision collision)
    {
        onCollisionStay.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnter.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        onTriggerExit.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        onTriggerStay.Invoke();
    }
}
